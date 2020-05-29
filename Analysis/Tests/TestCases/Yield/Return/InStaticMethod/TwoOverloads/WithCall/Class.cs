using System;
using System.Collections;

class Class {
	static IEnumerable Caller() {
		CalleeOfParameterless();

		yield return null;
	}

	static void CalleeOfParameterless() { }

	static IEnumerable Caller(
		ParameterClass parameter
	) {
		CalleeOfParameterized();

		yield return null;
	}

	static void CalleeOfParameterized() { }
}