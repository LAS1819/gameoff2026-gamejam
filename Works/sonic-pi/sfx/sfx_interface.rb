######################################
# SFX INTERFACE
######################################

use_bpm 120

######################################
# PARAMETERS
######################################

set :ui_amp, 0.8

######################################
# DEFINITIONS
######################################

### UI_CLICK ###
define :ui_click do |amp = get(:ui_amp)|

  use_synth :beep

  play :e5,
    amp: amp,
    release: 0.05

  sleep 0.05

  play :g5,
    amp: amp * 0.8,
    release: 0.05

end

######################################

### UI_DENIED ###
define :ui_denied do |amp = get(:ui_amp)|

  use_synth :beep

  play :g4,
    amp: amp,
    release: 0.08

  sleep 0.08

  play :e4,
    amp: amp,
    release: 0.12

end

######################################

### STORY_OPEN ###
define :story_open do |amp = get(:ui_amp)|

  with_fx :reverb,
    room: 0.8,
    mix: 0.4 do

    use_synth :pretty_bell

    play :e5,
      amp: amp,
      release: 0.15

    sleep 0.1

    play :g5,
      amp: amp,
      release: 0.2

    sleep 0.1

    play :b5,
      amp: amp,
      release: 0.4

  end

end

######################################
# LIVE LOOPS
######################################

live_loop :test_ui_click do
  stop

  ui_click

  sleep 1
end

######################################

live_loop :test_ui_denied do
  stop

  ui_denied

  sleep 1
end

######################################

live_loop :test_story_open do
  stop

  story_open

  sleep 2
end