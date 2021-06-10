using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Utils {

    public static class ListUtils {
        public static void RemoveLast<T>(this List<T> list, int offset = 0) {
            if (list == null || list.Count <= 0) {
                return;
            }

            list.RemoveAt(Mathf.Clamp(list.Count - 1 - offset, 0, list.Count - 1));
        }

        public static T Last<T>(this List<T> list, int offset = 0) {
            if (list == null || list.Count == 0) {
                return default;
            }

            offset = Mathf.Clamp(offset, 0, list.Count - 1);
            return list[list.Count - 1 - offset];
        }

        // source: https://stackoverflow.com/a/1262619
        public static void ShuffleUsingRandom<T>(this IList<T> list) {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // source: https://stackoverflow.com/a/1262619
        public static void ShuffleUsingCryptography<T>(this IList<T> list) {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1) {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void ShuffleUsingLinq<T>(this IList<T> list) {
            System.Random rng = new System.Random();
            list = list.OrderBy(a => rng.Next()).ToList();
        }
    }
}