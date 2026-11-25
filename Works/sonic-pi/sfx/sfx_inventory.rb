######################################
# SFX INVENTORY
######################################

use_bpm 80

######################################
# PARAMETERS
######################################

set :inventory_amp, 1.25

######################################
# DEFINITIONS
######################################

### ITEM_PICKUP ###
define :item_pickup do |amp = get(:inventory_amp)|

  use_synth :pretty_bell

  play :e5,
    amp: amp,
    release: 0.10

  sleep 0.06

  play :g5,
    amp: amp * 0.85,
    release: 0.12

  sleep 0.07

  play :b5,
    amp: amp * 0.65,
    release: 0.18

end

######################################

### ITEM_USE ###
define :item_use do |amp = get(:inventory_amp)|

  with_fx :echo,
    mix: 0.25,
    phase: 0.125 do

    use_synth :beep

    play :c5,
      amp: amp,
      attack: 0.01,
      sustain: 0.12,
      release: 0.16

  end

  sleep 0.10

  use_synth :pretty_bell

  play :g5,
    amp: amp * 0.65,
    release: 0.18

end

######################################

### LIGHT_SWITCH ###
define :light_switch do |amp = get(:inventory_amp)|

  use_synth :chipbass

  play :g4,
    amp: amp,
    release: 0.04

  sleep 0.02

  play :e4,
    amp: amp * 0.45,
    release: 0.06

end

######################################
# LIVE LOOPS
######################################

live_loop :test_item_pickup do
  stop

  item_pickup

  sleep 4
end

######################################

live_loop :test_item_use do
  stop

  item_use

  sleep 4
end

######################################

live_loop :test_light_switch do
  stop

  light_switch

  sleep 4
end

# SFX TEST ALL
live_loop :test_all_inventory_sfx do
  item_pickup
  sleep 8
  item_use
  sleep 8
  light_switch
  sleep 8
end
