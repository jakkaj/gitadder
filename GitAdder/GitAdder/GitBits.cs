using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitAdder
{
    public class GitBits
    {
        public void Scan()
        {
            var path = _getRepoPath();

            if (path == null)
            {
                Console.WriteLine("No GIT repository found");
            }

            using (var repo = new Repository(path))
            {
                foreach (var file in repo.Index)
                {
                    Console.WriteLine(file.Path);
                }

                _recurseAdd(repo, AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        void _recurseAdd(Repository repo, string path)
        {
            var dir = new DirectoryInfo(path);

            foreach (var file in dir.GetFiles())
            {
                
            }
        }

        string _getRepoPath()
        {
            var start = AppDomain.CurrentDomain.BaseDirectory;
            var directory = new DirectoryInfo(start);

            while (directory != null)
            {
                if (directory.GetDirectories().FirstOrDefault(_ => _.Name == ".git") != null)
                {
                    return directory.FullName;
                }
                directory = directory.Parent;
            }

            return null;


        }
    }
}
