######################################
# SFX WORLD EVENTS
######################################

use_bpm 80

######################################
# PARAMETERS
######################################

set :world_amp, 1.25

######################################
# DEFINITIONS
######################################

### MEMORY_DISCOVERY ###
define :memory_discovery do |amp = get(:world_amp)|

  with_fx :reverb,
    room: 0.9,
    mix: 0.55 do

    use_synth :pretty_bell

    play :e5,
      amp: amp,
      attack: 0.03,
      sustain: 0.12,
      release: 0.30

    sleep 0.12

    play :g5,
      amp: amp * 0.8,
      release: 0.36

    sleep 0.16

    play :b5,
      amp: amp * 0.60,
      release: 0.50

  end

end

######################################

### SHELF_SECRET_ACTION ###
define :shelf_secret_action do |amp = get(:world_amp)|

  use_synth :pluck

  play :c3,
    amp: amp,
    release: 0.12

  sleep 0.09

  synth :noise,
    amp: amp * 0.45,
    attack: 0.02,
    sustain: 0.08,
    release: 0.20

  sleep 0.10

  use_synth :pluck

  play :g2,
    amp: amp * 0.55,
    release: 0.16

end

######################################

### PRESSURE_VALVE_ACTION ###
define :pressure_valve_action do |amp = get(:world_amp)|

  use_synth :pluck

  play :e3,
    amp: amp,
    attack: 0.01,
    sustain: 0.10,
    release: 0.18

  sleep 0.12

  with_fx :hpf,
    cutoff: 65 do

    synth :noise,
      amp: amp * 0.70,
      attack: 0.02,
      sustain: 0.30,
      release: 0.35

  end

  sleep 0.08

  use_synth :beep

  play :c4,
    amp: amp * 0.35,
    release: 0.10

end

######################################
# LIVE LOOPS
######################################

live_loop :test_memory_discovery do
  stop

  memory_discovery

  sleep 4
end

######################################

live_loop :test_shelf_secret_action do
  stop

  shelf_secret_action

  sleep 4
end

######################################

live_loop :test_pressure_valve_action do
  stop

  pressure_valve_action

  sleep 4
end

# SFX TEST ALL
live_loop :test_all_world_event_sfx do
  memory_discovery
  sleep 8
  shelf_secret_action
  sleep 8
  pressure_valve_action
  sleep 8
end
