﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using NetFabric.Hyperlinq;

namespace eXtensionSharp {
    ref struct SplitSpanEnumerator {
        private ReadOnlySpan<char> text;
        private readonly char splitChar;
        
        public ReadOnlySpan<char> Current { get; private set; }
        
        public SplitSpanEnumerator(ReadOnlySpan<char> text, char splitChar) {
            this.text = text;
            this.splitChar = splitChar;
            this.Current = default;
        }

        public SplitSpanEnumerator GetEnumerator() => this;

        public bool MoveNext() {
            var index = text.IndexOf(splitChar);
            if (index == -1)
                return false;

            Current = text[..index];
            text = text[(index + 1)..];

            return true;
        }
    }
    
    public static class XString {
        /*
         * Span변환의 장점 : 스택에 메모리 할당되므로 GC가 발생하지 않도록 해줌.
         * Memory<T> (T:Class) -> Span<T> (T:Struct)로 변환하여 사용 가능
         * new 대신 stackalloc 사용할 경우 GC 압력이 줄어듬.(struct type에 대하여, int, char, byte 등등)
         * windows stack 최대 할당 용량은 1MB
         */
        public static string xSubstring(this string str, int startIndex, int length = 0) {
            if (length > 0) {
                return str.AsSpan()[startIndex..(startIndex + length)].ToString();
            }

            return str.AsSpan()[startIndex..str.Length].ToString();
        }

        public static IEnumerable<string> xSplit(this string str, char splitChar) {
            XList<string> result = new XList<string>();
            foreach (var @char in new SplitSpanEnumerator(str.AsSpan(), splitChar)) {
                result.Add(@char.ToString());
            }

            return result;
        }

        public static int xCount(this string str) {
            return str.xIsNull() ? 0 : str.Length;
        }

        public static bool xIsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        public static bool xIsNotNullOrEmpty(this string str) {
            return !string.IsNullOrEmpty(str);
        }

        public static string xReplace(this string text, string oldValue, string newValue) {
            return text.xIfNullOrEmpty(x => string.Empty).Replace(oldValue, newValue);
        }

        private static void xCopyTo(Stream src, Stream dest) {
            var bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) dest.Write(bytes, 0, cnt);
        }

        public static byte[] xFromStringToByteArray(this string str) {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                    //msi.CopyTo(gs);
                    xCopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string xToString(this byte[] bytes) {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                    //gs.CopyTo(mso);
                    xCopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string xJoin(this string[] value, string separator) {
            return string.Join(separator, value);
        }

        public static int xIndexOf(this string src, string value) {
            if (value.xIsNullOrEmpty()) return -1;
            return src.IndexOf(value);
        }

        public static int xIndexOfAny(this string src, string value) {
            if (value.xIsNullOrEmpty()) return -1;
            return src.IndexOfAny(value.ToCharArray());
        }

        public static int xLastIndexOf(this string src, string value) {
            if (value.xIsNullOrEmpty()) return -1;
            return src.LastIndexOf(value);
        }

        public static int xLastIndexOfAny(this string src, string value) {
            if (value.xIsNullOrEmpty()) return -1;
            return src.LastIndexOfAny(value.ToCharArray());
        }

        public static string xValue(this XStringBuilder xsb) {
            var str = string.Empty;
            xsb.Release(out str);
            return str.xTrim();
        }
    }
}