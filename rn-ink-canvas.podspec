require "json"

package = JSON.parse(File.read(File.join(__dir__, "package.json")))

Pod::Spec.new do |s|
  s.name         = "rn-ink-canvas"
  s.version      = package["version"]
  s.summary      = package["description"]
  s.description  = <<-DESC
                  rn-ink-canvas
                   DESC
  s.homepage     = "https://github.com/github_account/rn-ink-canvas"
  s.license      = "MIT"
  # s.license    = { :type => "MIT", :file => "FILE_LICENSE" }
  s.authors      = { "Your Name" => "yourname@email.com" }
  s.platform     = :ios, "7.0"
  s.source       = { :git => "https://github.com/github_account/rn-ink-canvas.git", :tag => "#{s.version}" }

  s.source_files = "ios/**/*.{h,m,swift}"
  s.requires_arc = true

  s.dependency "React"
	s.dependency 'AFNetworking', '~> 3.0'
  # s.dependency "..."
end

