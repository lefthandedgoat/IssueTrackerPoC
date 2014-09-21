#dont hate!

task :default => [:migrate]

task :migrate do
  sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe IssueTracker.sln'  
  sh 'migrations\bin\debug\migrations.exe'
end
