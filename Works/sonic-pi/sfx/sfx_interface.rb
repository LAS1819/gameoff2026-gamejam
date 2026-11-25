######################################
# SFX INTERFACE
######################################

use_bpm 80

######################################
# PARAMETERS
######################################

set :ui_amp, 1.25

######################################
# DEFINITIONS
######################################

### UI_CLICK ###
define :ui_click do |amp = get(:ui_amp)|
  
  #use_synth :beep
  #use_synth :pulse
  #use_synth :square
  use_synth :bass_foundation
  
  play :e5,
    amp: amp * 1.4,
    attack: 0.01,
    release: 0.05
  
  sleep 0.05
  
  play :g5,
    amp: amp,
    attack: 0.01,
    release: 0.05
  
end

######################################

### UI_DENIED ###
define :ui_denied do |amp = get(:ui_amp)|
  
  #use_synth :pulse
  use_synth :bass_foundation
  
  play :g5,
    amp: amp * 1.4,
    release: 0.05
  
  sleep 0.05
  
  play :e5,
    amp: amp,
    release: 0.08
  
end

######################################

### STORY_OPEN ###
define :story_open do |amp = get(:ui_amp)|
  
  use_synth :noise
  
  with_fx :hpf, cutoff: 90 do
    
    synth :noise,
      attack: 0.01,
      sustain: 0.04,
      release: 0.08,
      amp: amp * 0.25
    
  end
  
  sleep 0.03
  
  use_synth :bass_foundation
  
  play :e4,
    amp: amp * 0.4,
    release: 0.08
  
end

define :story_open_v2 do |amp = get(:ui_amp)|
  
  use_synth :noise
  
  synth :noise,
    attack: 0.001,
    sustain: 0.2,
    release: 0.2,
    #cutoff: 120,
    amp: amp * 0.22
  
  sleep 0.25
  
  #use_synth :bass_foundation
  use_synth :piano
  
  play 62,
    amp: amp,
    attack: 0.05,
    release: 1
  
end

######################################
# LIVE LOOPS
######################################

live_loop :test_ui_click do
  stop
  
  ui_click
  
  sleep 4
end

######################################

live_loop :test_ui_denied do
  stop
  
  ui_denied
  
  sleep 4
end

######################################

live_loop :test_story_open do
  stop
  
  #story_open
  story_open_v2
  
  sleep 4
end

# SFX TEST ALL
live_loop :test_all_interface_sfx do
  #stop
  ui_click
  sleep 8
  ui_denied
  sleep 8
  story_open_v2
  sleep 8
end
