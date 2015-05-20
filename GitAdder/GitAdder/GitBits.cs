//2015 Jordan Knight and Xamling in Sydney
//MS-PL (see https://github.com/jakkaj/gitadder)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitAdder
{
    public class GitBits
    {
        private readonly List<string> _trackedFiles = new List<string>();
        private string _basePath;

        readonly List<string> _excludes = new List<string>();
        readonly List<string> _includes = new List<string>();

        private bool _addedSomething = false;

        public bool Scan()
        {
            if (!_setupFilters())
            {
                return false;
            }

            _basePath = _getRepoPath();

            if (_basePath == null)
            {
                Console.WriteLine("No GIT repository found");
            }

            using (var repo = new Repository(_basePath))
            {
                foreach (var file in repo.Index)
                {
                    _trackedFiles.Add(file.Path);
                }

                _recurseAdd(repo, _basePath);
            }

            return _addedSomething;
        }

        bool _setupFilters()
        {
            var includeFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "includes.txt");
            var excludesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excludes.txt");

            if (!File.Exists(includeFile) || !File.Exists(excludesFile))
            {
                Console.WriteLine("Could not find includes.txt and/or excludes.txt. Write a regex on each line for files to include and exclude. Exclude wins the battle...");
                return false;
            }

            _loadFile(includeFile, _includes);
            _loadFile(excludesFile, _excludes);
            
            return true;
        }

        void _loadFile(string file, List<string> list)
        {
            var data = File.ReadAllText(file);

            var lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            list.AddRange(lines.Where(line => !line.StartsWith("#")));
        }

        void _recurseAdd(Repository repo, string path)
        {
            var dir = new DirectoryInfo(path);

            foreach (var file in dir.GetFiles())
            {
                var adjustedFile = file.FullName.Replace(_basePath, "").Trim('\\');
                _checkTracking(repo, adjustedFile);
            }

            foreach (var child in dir.GetDirectories())
            {
                _recurseAdd(repo, child.FullName);
            }
        }

        void _checkTracking(Repository repo, string file)
        {
            if (!_trackedFiles.Contains(file))
            {
                if (_checkInclues(file) && !_checkExcludes(file))
                {
                    //add it
                    repo.Index.Add(file);
                    Console.WriteLine($"Added {file}");
                    _addedSomething = true;
                }
            }
        }

        bool _checkInclues(string file)
        {
            return _includes.Any(include => Regex.IsMatch(file, include));
        }

        bool _checkExcludes(string file)
        {
            return _excludes.Any(exclude => Regex.IsMatch(file, exclude));
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
