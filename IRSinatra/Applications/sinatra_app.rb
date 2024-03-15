require 'sinatra'

get '/' do
  "Try /hi"
end

get '/hi' do
  "Hello Embedded IronRuby World!"
end
