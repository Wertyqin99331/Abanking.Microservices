using System.Reflection;

namespace Core.Helpers;

public static class AssemblyHelper
{
	public static IEnumerable<Assembly> GetAllAssembliesWithoutDefaultAssemblies()
	{
		var usedAssemblies = new HashSet<string>();
		var stack = new Stack<Assembly>();

		stack.Push(Assembly.GetEntryAssembly()!);

		do
		{
			var asm = stack.Pop();

			yield return asm;

			foreach (var reference in asm.GetReferencedAssemblies())
				if (!usedAssemblies.Contains(reference.FullName) && !reference.FullName.StartsWith("System") &&
				    !reference.FullName.StartsWith("Microsoft"))
				{
					stack.Push(Assembly.Load(reference));
					usedAssemblies.Add(reference.FullName);
				}
		} while (stack.Count > 0);
	}
}