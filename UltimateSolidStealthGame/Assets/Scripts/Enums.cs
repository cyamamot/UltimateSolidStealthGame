using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums {

	public enum directions {
		up,
		down,
		left,
		right
	};

    public static int RandomIntExcept(int min, int max, int except) {
        int result = Random.Range(min, max - 1);
        if (result >= except) result += 1;
        return result;
    }

    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        System.Random rnd = new System.Random();
        while (n > 1) {
            int k = (rnd.Next(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


}
