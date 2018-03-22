using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace Aiv.Mpg123
{
    public class Mpg123 : IDisposable
    {
        public class ErrorException : Exception
        {
            public ErrorException(Errors error) : base(PlainStrError(error))
            {
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct mpg123_id3_v1
        {
            char[] tag;
            char[] title;
            char[] artist;
            char[] album;
            char[] year;
            char[] comment;
            char genre;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct mpg123_id3_v2
        {
            char version;
            IntPtr title;
            IntPtr artist;
            IntPtr album;
            IntPtr year;
            IntPtr genre;
            IntPtr comment;
            IntPtr comment_list;
            IntPtr comments;
            IntPtr text;
            IntPtr texts;
            IntPtr extra;
            IntPtr extras;
            IntPtr picture;
            IntPtr pictures;
        }

        public enum mpg123_id3_enc
        {
            mpg123_id3_latin1 = 0,
            mpg123_id3_utf16bom = 1,
            mpg123_id3_utf16be = 2,
            mpg123_id3_utf8 = 3,
            mpg123_id3_enc_max = 4
        }

        public enum Errors
        {
            OK = 0,
        }

        static private bool libraryInitialized;
        static public bool IsLibraryInitialized
        {
            get
            {
                return libraryInitialized;
            }
        }

        public static IEnumerable<string> Decoders
        {
            get
            {
                IntPtr decodersPtr = NativeMethods.NativeMpg123Decoders();
                int offset = 0;
                while (true)
                {
                    IntPtr decoderPtr = Marshal.ReadIntPtr(decodersPtr, offset);
                    if (decoderPtr == IntPtr.Zero)
                    {
                        yield break;
                    }
                    yield return Marshal.PtrToStringAnsi(decoderPtr);
                    offset += Marshal.SizeOf<IntPtr>();
                }
            }
        }

        public static string PlainStrError(Errors error)
        {
            IntPtr errorPtr = NativeMethods.NativeMpg123PlainStrError(error);
            if (errorPtr == IntPtr.Zero)
                return "unknown error";
            string errorMessage = Marshal.PtrToStringAnsi(errorPtr);
            return errorMessage;
        }

        static Mpg123()
        {
            Errors error = NativeMethods.NativeMpg123Init();
            if (error != Errors.OK)
                throw new ErrorException(error);
            libraryInitialized = true;
        }

        public bool HasValidHandle
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        protected IntPtr handle;

        public Mpg123(string decoder = null)
        {
            IntPtr decoderPtr = IntPtr.Zero;
            if (decoder != null)
            {
                decoderPtr = Marshal.StringToHGlobalAnsi(decoder);
            }
            int error = 0;
            handle = NativeMethods.NativeMpg123New(decoderPtr, ref error);
            if (decoderPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(decoderPtr);
            if (handle == IntPtr.Zero)
                throw new ErrorException((Errors)error);
        }

        protected bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool isDisposing)
        {
            if (disposed)
                return;

            if (handle != IntPtr.Zero)
            {
                NativeMethods.NativeMpg123Delete(handle);
                handle = IntPtr.Zero;
            }

            if (isDisposing)
            {
                // cleanup dependancies
            }

            disposed = true;
        }

        public int MetaCheck(long mh)
        {
            int error = 0;
            int metacheck = NativeMethods.NativeMpg123MetaCheck((IntPtr)mh);
            if (metacheck == 0)
            {
                throw new ErrorException((Errors) error);
            }
            return metacheck;
        }

        public void MetaFree(long mh)
        {
            NativeMethods.NativeMpg123MetaFree((IntPtr)mh);
        }

        public int Id3(long mh, ref long v1, ref long v2)
        {
            IntPtr v3 = (IntPtr)v1;
            int id3 = NativeMethods.NativeMpg123Id3((IntPtr)mh,ref v3, (IntPtr)v2);
            return id3;
        }

        public int Id3Raw(long mh, ref long v1, long v1_size, ref long v2, long v2_size)
        {
            IntPtr v3 = (IntPtr)v1;
            IntPtr v4 = (IntPtr)v2;
            int id3raw = NativeMethods.NativeMpg123Id3Raw((IntPtr)mh, ref v3, (IntPtr)v1_size, ref v4, (IntPtr)v2_size);
            Errors error = NativeMethods.NativeMpg123Init();
            if (error != Errors.OK)
                throw new ErrorException(error);
            else
            {
                return id3raw;      
            }
        }

        public int Icy(long mh, ref long icy_meta)
        {
            IntPtr v1 = (IntPtr)icy_meta;
            int icy = NativeMethods.NativeMpg123Icy((IntPtr)mh, ref v1);
            Errors error = NativeMethods.NativeMpg123Init();
            if (error != Errors.OK)
                throw new ErrorException(error);
            else
            {
                return icy;
            }
        }

        public IntPtr Icy2Utf8(long icy_text)
        {
            return NativeMethods.NativeMpg123Icy2Utf8((IntPtr)icy_text);
        }

        ~Mpg123()
        {
            Dispose(false);
        }
    } 
}
