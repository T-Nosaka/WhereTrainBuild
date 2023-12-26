using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.CSharp;
using System.Windows.Forms;

namespace WhereTrainBuild.MapUtil.Data
{
    /// <summary>
    /// 動的ビルダー
    /// </summary>
    public class DynamicBuildLogic
    {
        /// <summary>
        /// SetDllDirectory
        /// </summary>
        /// <param name="lpPathName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DynamicBuildLogic()
        {

        }

        /// <summary>
        /// 構築
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="filename">スクリプトファイル</param>
        /// <returns></returns>
        public bool BuildDaia(BaseFactory factory, string filename, Control cnt )
        {
            //解凍
            var basefolder = Path.Combine(Path.GetDirectoryName(filename),"extend");
            try
            {
                ZipFile.ExtractToDirectory(filename, basefolder);

                //ソース一覧
                var srclist = new List<String>();
                foreach (var srcfile in Directory.GetFiles(basefolder, "*.cs"))
                {
                    //ソースロード
                    using (StreamReader streamReader = new StreamReader(srcfile, Encoding.UTF8))
                    {
                        string scriptsrc = streamReader.ReadToEnd();
                        //スクリプトリスト追加
                        srclist.Add(scriptsrc);
                    }
                }

                //依存ライブラリ
                if (Directory.Exists(Path.Combine(basefolder, "native")) == true)
                {
                    SetDllDirectory(Path.Combine(basefolder, "native"));
                }

                //依存アセンブリ
                var reffolder = System.AppDomain.CurrentDomain.BaseDirectory;
                var asslist = new List<String>();
                foreach (var srcfile in Directory.GetFiles(basefolder, "*.dll"))
                {
                    var destfile = Path.Combine(reffolder, Path.GetFileName(srcfile));
                    if( File.Exists(destfile) == false )
                        File.Copy(srcfile, destfile);
                    asslist.Add(destfile);
                }

                if (Compile(srclist, asslist) == true)
                {
                    var result = (string)Run(factory, basefolder, cnt);
                    if (result == "OK")
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch( Exception ex )
            {
                m_exception = ex;

                return false;
            }
            finally
            {
                try
                {
                    Directory.Delete(basefolder, true);
                }
                catch { }
            }
        }

        #region 動的コンパイル
        /// <summary>
        /// コンパイル結果
        /// </summary>
        protected CompilerResults m_compilerResults = null;

        /// <summary>
        /// 例外
        /// </summary>
        protected Exception m_exception = null;

        /// <summary>
        /// エラー文字出力
        /// </summary>
        /// <returns>エラーメッセージ</returns>
        public string ErrorToString()
        {
            string error = string.Empty;

            if (m_compilerResults != null)
            {
                foreach (CompilerError ce in m_compilerResults.Errors)
                {
                    error += string.Format("[{0}]:[{1}]\r", ce.Line, ce.ErrorText);
                }
            }
            if( m_exception != null )
            {
                error += m_exception.ToString();
            }

            return error;
        }

        /// <summary>
        /// 読み込んでいるアセンブリファイルリストを取得する
        /// </summary>
        /// <param name="domain">対象ドメイン</param>
        /// <returns>アセンブリファイルリスト</returns>
        protected static List<string> GetAssemblyFile(AppDomain domain)
        {
            List<string> filelist = new List<string>();

            //実行環境を含める
            foreach (Assembly asm in domain.GetAssemblies())
            {
                try
                {
                    foreach (FileStream fs in asm.GetFiles())
                    {
                        string filename = fs.Name;
                        if (File.Exists(filename) == true)
                            filelist.Add(filename);
                    }
                }
                catch
                {
                    //スクリプトコンパイルした物が、例外発生
                }
            }

            return filelist;
        }

        /// <summary>
        /// コンパイル
        /// </summary>
        /// <param name="srclist">スクリプトソースリスト</param>
        /// <param name="assemblyNames">外部アセンブリリスト</param>
        /// <returns>True..成功 False..失敗</returns>
        protected bool Compile(List<string> srclist, List<string> assemblyfiles)
        {
            List<string> asmlist = GetAssemblyFile(AppDomain.CurrentDomain);

            //だぶりをフィルタする
            Dictionary<string, string> asmtbl = new Dictionary<string, string>();
            foreach (string assemlyfile in asmlist)
            {
                //除外
                if (assemlyfile.IndexOf("System.EnterpriseServices.Wrapper.dll") >= 0)
                    continue;

                asmtbl[Path.GetFileName(assemlyfile)] = assemlyfile;
            }

            //実行環境に不足しているアセンブリをロードする。
            foreach( var dll in assemblyfiles)
            {
                if (asmtbl.ContainsKey(dll) == false )
                    Assembly.LoadFrom(dll);
            }

            // コンパイル時のオプション設定
            CompilerParameters param = new CompilerParameters();
            param.GenerateInMemory = true;
            param.IncludeDebugInformation = false;

            foreach ( var dr in asmtbl)
            {
                assemblyfiles.Add(dr.Value);
            }

            //実行環境を含める
            foreach (string asmblyfile in assemblyfiles)
            {
                param.ReferencedAssemblies.Add(asmblyfile);
            }

            // コンパイルする
            using (CSharpCodeProvider codeProvider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } }))
            {
                m_compilerResults = codeProvider.CompileAssemblyFromSource(param, srclist.ToArray());

                // エラーメッセージが無ければ成功
                return !m_compilerResults.Errors.HasErrors;
            }
        }

        /// <summary>
        /// コンパイルしたアセンブリ
        /// </summary>
        /// <returns>
        /// アセンブリ
        /// </returns>
        protected Assembly GetAssembly()
        {
            return m_compilerResults.CompiledAssembly;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="factory">ファクトリ</param>
        /// <param name="basefolder">ベースフォルダ</param>
        /// <returns>
        /// スクリプトからの戻値
        /// </returns>
        protected object Run(BaseFactory factory, string basefolder, Control cnt)
        {
            return Run(factory, basefolder, cnt, GetAssembly());
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="arglist">スクリプト引数</param>
        /// <param name="entryclass">エントリクラス名</param>
        /// <param name="assembly">対象アセンブリ</param>
        /// <returns>戻値</returns>
        protected static object Run(BaseFactory factory, String basefolder, Control cnt, Assembly assembly)
        {
            object scriptengine = assembly.CreateInstance("WhereTrainBuild.Script.TransitBuildLogic");
            if (scriptengine == null)
                return null;

            Type type = scriptengine.GetType();
            MethodInfo mi = type.GetMethod("Build");

            try
            {
                return mi.Invoke(scriptengine, new object[] { factory, basefolder, cnt });
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
        #endregion
    }
}
