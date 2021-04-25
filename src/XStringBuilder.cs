using System;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace eXtensionSharp {
    /// <summary>
    /// StringBuilder Pool
    /// 메모리 소비를 늘리는 대신 GC를 줄입니다.
    /// </summary>
    public class XStringBuilder : IDisposable {
        private ObjectPool<StringBuilder> _stringBuilderPool = null;
        private StringBuilder _stringBuilder = null;
        
        public XStringBuilder() {
            var objectPoolProvider = new DefaultObjectPoolProvider();
            _stringBuilderPool = objectPoolProvider.CreateStringBuilderPool();
            _stringBuilder = _stringBuilderPool.Get();
        }

        public XStringBuilder(int capacity) {
            var objectPoolProvider = new DefaultObjectPoolProvider();
            _stringBuilderPool = objectPoolProvider.CreateStringBuilderPool(capacity, capacity * 2);
            _stringBuilder = _stringBuilderPool.Get();
        }

        public XStringBuilder(int initCapacity, int maxCapacity) {
            var objectPoolProvider = new DefaultObjectPoolProvider();
            _stringBuilderPool = objectPoolProvider.CreateStringBuilderPool(initCapacity, maxCapacity);
            _stringBuilder = _stringBuilderPool.Get();
        }

        public void Append(string str) {
            _stringBuilder.Append(str);
        }

        public void Append(char c) {
            _stringBuilder.Append(c);
        }

        public void AppendLine(string str) {
            _stringBuilder.AppendLine(str);
        }

        public void AppendFormat(CultureInfo cultureInfo, string format, params object[] objs) {
            _stringBuilder.AppendFormat(cultureInfo, format, objs);
        }

        public void AppendJoin(string seperator, params object[] objs) {
            _stringBuilder.AppendJoin(seperator, objs);
        }

        public void Dispose() {
            
        }

        public void Release(out string str) {
            str = _stringBuilder.ToString();
            _stringBuilderPool.Return(_stringBuilder);
            _stringBuilderPool = null;
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}