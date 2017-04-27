using System;
using System.Threading.Tasks;

namespace DuluthHomegrown2017
{
	/// <summary>
	/// An interface for getting the string contents of a file that is in an app bundle.
	/// </summary>
	public interface ILocalBundleFileManager
	{
		string ReadFileFromBundleAsString(string fileName);
	}
}
