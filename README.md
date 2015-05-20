# gitadder
Little app to recursively search and add files to Git repositories following simple include and exclude rules

Rules are added to the include.txt and exclude.txt files. They are regex! Excludes win over includes. 

The app searches up until it can find a .git repo -> this becomes its root dir, from which it recursively searches down to find missing files. 

It's great for when Visual Studio misses your files, just run it and off you go :)

It makes use of https://github.com/libgit2/libgit2sharp (cheers for that!)

There is a Nuget package available here: https://www.nuget.org/packages/GitAdder/1.0.0