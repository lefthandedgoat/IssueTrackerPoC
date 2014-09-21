#dont hate!

task :default => [:migrate]

task :migrate do
  sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe IssueTracker\IssueTracker.sln'  
  sh 'IssueTracker\migrations\bin\debug\migrations.exe'
end
