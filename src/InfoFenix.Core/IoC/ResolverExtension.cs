namespace InfoFenix.IoC {
    public static class ResolverExtension {

        #region Public Static Methods

        public static T Resolve<T>(this IResolver source, string name = null) {
            if (source == null) { return default(T); }

            return (T)source.Resolve(serviceType: typeof(T), name: name);
        }

        #endregion Public Static Methods
    }
}