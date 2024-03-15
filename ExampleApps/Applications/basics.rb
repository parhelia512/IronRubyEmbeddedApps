require 'pp'

puts "----------------- $LOAD_PATH ---------------------"
pp $LOAD_PATH

puts "----------------- Constants ----------------------"
puts "RUBY_ENGINE = #{RUBY_ENGINE}"
puts "RUBY_VERSION = #{RUBY_VERSION}"
puts "RUBY_PATCHLEVEL = #{RUBY_PATCHLEVEL}"
puts "RUBY_PLATFORM = #{RUBY_PLATFORM}"
puts "RUBY_RELEASE_DATE = #{RUBY_RELEASE_DATE}"
puts "RUBY_DESCRIPTION = #{RUBY_DESCRIPTION}"
puts "VERSION = #{VERSION}"
puts "PLATFORM = #{PLATFORM}"
puts "RELEASE_DATE = #{RELEASE_DATE}"
puts "IRONRUBY_PLATFORM = #{IRONRUBY_PLATFORM}"
puts "IRONRUBY_VERSION = #{IRONRUBY_VERSION}"

puts "----------------- Encodings ----------------------"
HTML5ASCIIINCOMPAT =  if (IRONRUBY_PLATFORM =~ /Framework/)
                        [Encoding::UTF_7, Encoding::UTF_16BE, Encoding::UTF_16LE, Encoding::UTF_32BE, Encoding::UTF_32LE] # :nodoc:
                      else
                        # Encoding::UTF_7 has been removed for .NET5
                        [Encoding::UTF_16BE, Encoding::UTF_16LE, Encoding::UTF_32BE, Encoding::UTF_32LE] # :nodoc:
                      end
pp HTML5ASCIIINCOMPAT

# pp methods

puts "---------------- require('fcntl') ----------------"
p require('fcntl')

puts "----------------- methods ------------------------"
p methods
