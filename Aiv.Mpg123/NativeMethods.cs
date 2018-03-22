using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Aiv.Mpg123
{
    static class NativeMethods
    {
        internal const string LibraryName = "libmpg123-0.dll";

        [DllImport(LibraryName, EntryPoint = "mpg123_init", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal extern static Mpg123.Errors NativeMpg123Init();

        [DllImport(LibraryName, EntryPoint = "mpg123_plain_strerror", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123PlainStrError([MarshalAs(UnmanagedType.I4)] Mpg123.Errors error);

        [DllImport(LibraryName, EntryPoint = "mpg123_decoders", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123Decoders();

        [DllImport(LibraryName, EntryPoint = "mpg123_new", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123New(IntPtr decoder, ref int error);

        [DllImport(LibraryName, EntryPoint = "mpg123_delete", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123Delete(IntPtr handle);

        //-----------------

        [DllImport(LibraryName, EntryPoint = "mpg123_meta_check", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123MetaCheck(IntPtr mh);

        [DllImport(LibraryName, EntryPoint = "mpg123_meta_free", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void NativeMpg123MetaFree(IntPtr mh);

        [DllImport(LibraryName, EntryPoint = "mpg123_id3_", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123Id3(IntPtr mh, ref IntPtr v1, IntPtr v2);

        [DllImport(LibraryName, EntryPoint = "mpg123_id3_raw", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123Id3Raw(IntPtr mh,ref IntPtr v1, IntPtr v1_size, ref IntPtr v2, IntPtr v2_size);

        [DllImport(LibraryName, EntryPoint = "mpg123_icy", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int NativeMpg123Icy(IntPtr mh, ref IntPtr icy_meta);

        [DllImport(LibraryName, EntryPoint = "mpg123_icy2utf8", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr NativeMpg123Icy2Utf8(IntPtr icy_text);
    }
}
