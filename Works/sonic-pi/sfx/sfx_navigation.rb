######################################
# SFX NAVIGATION
######################################

use_bpm 80

######################################
# PARAMETERS
######################################

set :navigation_amp, 1.25

######################################
# DEFINITIONS
######################################

### SHIFT ###
define :shift do |amp = get(:navigation_amp)|
  
  with_fx :reverb,
    room: 0.5,
  mix: 0.35 do
    
    #use_synth :blade
    use_synth :noise
    #use_synth :bnoise
    
    play :c3,
      amp: amp,
      attack: 0.02,
      sustain: 0.12,
      release: 0.18
    
    sleep 0.10
    
    play :g3,
      amp: amp * 0.9,
      attack: 0.02,
      sustain: 0.15,
      release: 0.25
    
    sleep 0.12
    
    play :c4,
      amp: amp * 0.7,
      attack: 0.02,
      sustain: 0.10,
      release: 0.30
    
  end
  
end

define :shift_v2 do |amp = get(:navigation_amp)|
  
  with_fx :hpf, cutoff: 90 do
    
    synth :noise,
      attack: 0.01,
      sustain: 0.18,
      release: 0.15,
      amp: amp * 0.2
    
  end
  
end

define :shift_v3 do |amp = get(:navigation_amp)|
  
  with_fx :hpf, cutoff: 90 do
    
    synth :noise,
      attack: 0.01,
      sustain: 0.12,
      release: 0.15,
      amp: amp * 0.15
    
  end
  
  sleep 0.04
  
  use_synth :bass_foundation
  
  play :e3,
    amp: amp * 0.15,
    attack: 0.01,
    release: 0.10
  
end

define :shift_v4 do |amp = get(:navigation_amp)|
  
  with_fx :reverb,
    room: 0.6,
  mix: 0.2 do
    
    # Fffsshh...
    synth :noise,
      attack: 0.01,
      sustain: 0.24,
      release: 0.18,
      amp: amp * 0.12
    
    sleep 0.02
    
    # ...uuui
    use_synth :bass_foundation
    
    play :c2,
      amp: amp * 0.20,
      attack: 0.01,
      release: 0.30
    
    sleep 0.03
    
    play 40,
      amp: amp * 0.29,
      attack: 0.01,
      release: 0.40
    
  end
  
end

######################################

### ROOM_TRANSITION ###
define :room_transition do |amp = get(:navigation_amp)|
  
  with_fx :hpf,
  cutoff: 55 do
    
    with_fx :reverb,
      room: 0.7,
    mix: 0.5 do
      
      synth :noise,
        amp: amp,
        attack: 0.06,
        sustain: 0.18,
        release: 0.34
      
    end
    
  end
  
  sleep 0.12
  
  use_synth :beep
  
  play :e3,
    amp: amp * 0.6,
    release: 0.20
  
end

define :room_transition_v2 do |amp = get(:navigation_amp)|
  
  #use_synth :bass_foundation
  use_synth  :noise
  
  play :e3,
    amp: amp * 0.60,
    release: 0.08
  
  sleep 0.2
  
  play :d3,
    amp: amp * 0.48,
    release: 0.08
  
  sleep 0.2
  
  play :c3,
    amp: amp * 0.38,
    release: 0.12
  
end

######################################

### DOOR_ACTION ###
define :door_action do |amp = get(:navigation_amp)|
  
  use_synth :pluck
  
  play :c3,
    amp: amp,
    release: 0.10
  
  sleep 0.07
  
  play :g3,
    amp: amp * 0.75,
    release: 0.12
  
  sleep 0.08
  
  play :e3,
    amp: amp * 0.55,
    release: 0.18
  
end

define :door_action_v2 do |amp = get(:navigation_amp)|
  
  use_synth :noise
  
  play :c3,
    amp: amp * 0.45,
    attack: 0.01,
    release: 0.05
  
  sleep 0.1
  
  use_synth :bass_foundation
  
  play :c2,
    amp: amp * 0.55,
    attack: 0.01,
    release: 0.12
  
  sleep 0.06
  
  play :d2,
    amp: amp * 0.45,
    attack: 0.01,
    sustain: 0.25,
    release: 0.4
  
end

######################################
# LIVE LOOPS
######################################

live_loop :test_shift do
  stop
  
  #shift
  #shift_v2
  #shift_v3
  shift_v4
  
  sleep 4
end

######################################

live_loop :test_room_transition do
  stop
  
  #room_transition
  room_transition_v2
  
  sleep 4
end

######################################

live_loop :test_door_action do
  #stop
  
  #door_action
  door_action_v2
  
  sleep 4
end

# SFX TEST ALL
live_loop :test_all_navigation_sfx do
  stop
  shift
  sleep 8
  room_transition
  sleep 8
  door_action
  sleep 8
end
