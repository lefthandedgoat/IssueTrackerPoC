#dont hate!

task :default => [:build, :one]

task :build do
  sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe IssueTracker.sln'  
end

task :migrate  do
  sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe migrations\migrations.fsproj'  
  sh 'migrations\bin\debug\migrations.exe'
end

task :one do
  sh 'tests\bin\debug\tests.exe ui one'
end

task :many do
  sh 'tests\bin\debug\tests.exe ui many'
end