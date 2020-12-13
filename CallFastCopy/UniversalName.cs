using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NFsCGI
{
	public class UniversalName
	{
		#region UN
		[DllImport("mpr.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.U4)]
		static extern int
	   WNetGetUniversalName(
		   string lpLocalPath,                                 // ネットワーク資源のパス 
		   [MarshalAs(UnmanagedType.U4)] int dwInfoLevel,      // 情報のレベル
		   IntPtr lpBuffer,                                    // 名前バッファ
		   [MarshalAs(UnmanagedType.U4)] ref int lpBufferSize  // バッファのサイズ
	   );
		const int UNIVERSAL_NAME_INFO_LEVEL = 0x00000001;
		const int REMOTE_NAME_INFO_LEVEL = 0x00000002; //こちらは、テストしていない
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		struct UNIVERSAL_NAME_INFO
		{
			public string lpUniversalName;
		}
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		struct _REMOTE_NAME_INFO  //こちらは、テストしていない
		{
			string lpUniversalName;
			string lpConnectionName;
			string lpRemainingPath;
		}
		const int NO_ERROR = 0;
		const int ERROR_NOT_SUPPORTED = 50;
		const int ERROR_MORE_DATA = 234;
		const int ERROR_BAD_DEVICE = 1200;
		const int ERROR_CONNECTION_UNAVAIL = 1201;
		const int ERROR_NO_NET_OR_BAD_PATH = 1203;
		const int ERROR_EXTENDED_ERROR = 1208;
		const int ERROR_NO_NETWORK = 1222;
		const int ERROR_NOT_CONNECTED = 2250;

		public static string GetName(string path_src)
		{
			string unc_path_dest = path_src; //解決できないエラーが発生した場合は、入力されたパスをそのまま戻す
			int size = 1;

			/*
			 * 前処理
			 *   意図的に、ERROR_MORE_DATAを発生させて、必要なバッファ・サイズ(size)を取得する。
			 */
			//1バイトならば、確実にERROR_MORE_DATAが発生するだろうという期待。
			IntPtr lp_dummy = Marshal.AllocCoTaskMem(size);

			//サイズ取得をトライ
			int apiRetVal = WNetGetUniversalName(path_src, UNIVERSAL_NAME_INFO_LEVEL, lp_dummy, ref size);

			//ダミーを解放
			Marshal.FreeCoTaskMem(lp_dummy);


			/*
			 * UNC変換処理
			 */
			switch (apiRetVal)
			{
				case ERROR_MORE_DATA:
					//受け取ったバッファ・サイズ(size)で再度メモリ確保
					IntPtr lpBufUniversalNameInfo = Marshal.AllocCoTaskMem(size);

					//UNCパスへの変換を実施する。
					apiRetVal = WNetGetUniversalName(path_src, UNIVERSAL_NAME_INFO_LEVEL, lpBufUniversalNameInfo, ref size);

					//UNIVERSAL_NAME_INFOを取り出す。
					UNIVERSAL_NAME_INFO a = (UNIVERSAL_NAME_INFO)Marshal.PtrToStructure(lpBufUniversalNameInfo, typeof(UNIVERSAL_NAME_INFO));

					//バッファを解放する
					Marshal.FreeCoTaskMem(lpBufUniversalNameInfo);

					if (apiRetVal == NO_ERROR)
					{
						//UNCに変換したパスを返す
						unc_path_dest = a.lpUniversalName;
					}
					else
					{
						//MessageBox.Show(path_src +"ErrorCode:" + apiRetVal.ToString());
					}
					break;

				case ERROR_BAD_DEVICE: //すでにUNC名(\\servername\test)
				case ERROR_NOT_CONNECTED: //ローカル・ドライブのパス(C:\test)
										  //MessageBox.Show(path_src +"\nErrorCode:" + apiRetVal.ToString());
					break;
				default:
					//MessageBox.Show(path_src + "\nErrorCode:" + apiRetVal.ToString());
					break;

			}

			return unc_path_dest;
		}
		#endregion

	}
}
