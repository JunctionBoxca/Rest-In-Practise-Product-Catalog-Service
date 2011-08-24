require 'rubygems'
require 'albacore'
require 'rake/clean'

BASE_DIR=File.dirname(__FILE__)
OUTPUT_DIR="#{BASE_DIR}/output"
FILENAME_EXE="ProductCatalog.Host.exe"
OUTPUT_EXE="#{OUTPUT_DIR}/#{FILENAME_EXE}"
BIN_DIR="#{BASE_DIR}/src/ProductCatalog.Host/bin/Debug"
BIN_EXE="#{BASE_DIR}/src/ProductCatalog.Host/bin/Debug/#{FILENAME_EXE}"
PACKAGE_SET=FileList["#{BIN_DIR}/**/*.exe","#{BIN_DIR}/**/*.exe.config","#{BIN_DIR}/**/*.dll"]
NUNIT_CONSOLE="#{BASE_DIR}/tools/nunit/nunit-console.exe"
TEST_ASSEMBLIES="#{BASE_DIR}/src/ProductCatalog.Tests/bin/Debug/ProductCatalog.Tests.dll"
SOLUTION_FILE="#{BASE_DIR}/Restbucks.ProductCatalog.sln"
CLEAN << OUTPUT_DIR


task :default => :build


msbuild :clean do |msb|
	msb.use :net35
	msb.targets :Clean
	msb.solution = SOLUTION_FILE
end


desc 'Compile the application and unit tests.'
msbuild :compile do |msb|
	msb.use :net35
	msb.targets :Build
	msb.solution = SOLUTION_FILE
end


desc 'Package the application into the output folder.'
task :package => [:build] do
	mkdir OUTPUT_DIR unless File.exists? OUTPUT_DIR
	PACKAGE_SET.each do |file|
		cp file, OUTPUT_DIR
	end
end


desc 'Build the system.'
task :build => [:clean, :compile, :test]

desc 'Run unit tests.'
nunit :test => :compile do |nunit|
	nunit.command = NUNIT_CONSOLE
	nunit.assemblies TEST_ASSEMBLIES
end

desc 'Run the Product Catalogue application.'
task :run => :compile do
	%x{start "Product Catalogue" "#{BIN_EXE}"} 
end
