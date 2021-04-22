namespace eXtensionSharp {
    public static class XCastExtension {
        public static TDest xCast<TDest>(this object src)
            where TDest : class {
            return src as TDest;
        }

        public static TDest xCast<TSrc, TDest>(this TSrc src)
            where TSrc : class
            where TDest : class {
            return src as TDest;
        }
    }
}