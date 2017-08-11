using System;
using System.Collections;
using System.Collections.Generic;

namespace InfoFenix.Core {

    /// <summary>
    /// Extension methods for <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtension {

        #region Public Static Methods

        /// <summary>
        /// Interact throught an instance of <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The enumerable argument type.</typeparam>
        /// <param name="source">An instance of <see cref="IEnumerable{T}"/>.</param>
        /// <param name="action">The interator action.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="source"/> or <paramref name="action"/> were <c>null</c>.
        /// </exception>
        public static void Each<T>(this IEnumerable<T> source, Action<T> action) {
            Prevent.ParameterNull(action, nameof(action));

            Each(source, (current, _) => action(current));
        }

        /// <summary>
        /// Interact throught an instance of <see cref="IEnumerable{T}"/>.
        /// And pass an index value to the interator action.
        /// </summary>
        /// <typeparam name="T">The enumerable argument type.</typeparam>
        /// <param name="source">An instance of <see cref="IEnumerable{T}"/>.</param>
        /// <param name="action">The interator action.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="source"/> or <paramref name="action"/> were <c>null</c>.
        /// </exception>
        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action) {
            Prevent.ParameterNull(action, nameof(action));

            if (source == null) { return; }

            var counter = 0;

            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    action(enumerator.Current, counter++);
                }
            }
        }

        /// <summary>
        /// Interact throught an instance of <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="source">An instance of <see cref="IEnumerable"/>.</param>
        /// <param name="action">The interator action.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="source"/> or <paramref name="action"/> were <c>null</c>.
        /// </exception>
        public static void Each(this IEnumerable source, Action<object> action) {
            Prevent.ParameterNull(action, nameof(action));

            Each(source, (current, _) => action(current));
        }

        /// <summary>
        /// Interact throught an instance of <see cref="IEnumerable"/>.
        /// And pass an index value to the interator action.
        /// </summary>
        /// <param name="source">An instance of <see cref="IEnumerable"/>.</param>
        /// <param name="action">The interator action.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="source"/> or <paramref name="action"/> were <c>null</c>.
        /// </exception>
        public static void Each(this IEnumerable source, Action<object, int> action) {
            Prevent.ParameterNull(action, nameof(action));

            if (source == null) { return; }

            var counter = 0;
            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext()) {
                action(enumerator.Current, counter++);
            }

            var disposable = enumerator as IDisposable;

            if (disposable != null) {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Checks if an <see cref="IEnumerable"/> is empty.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable"/> instance.</param>
        /// <returns><c>true</c>, if is empty, otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="source"/> is <c>null</c>.</exception>
        public static bool IsNullOrEmpty(this IEnumerable source) {
            if (source == null) { return true; }

            var enumerator = source.GetEnumerator();
            var canMoveNext = enumerator.MoveNext();
            var disposable = enumerator as IDisposable;

            if (disposable != null) {
                disposable.Dispose();
            }

            return !canMoveNext;
        }

        #endregion Public Static Methods
    }
}