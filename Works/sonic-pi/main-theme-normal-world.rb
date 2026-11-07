############################################
#
# GAME OFF 2026
# MAIN THEME - NORMAL WORLD / OTHER SIDE
#
############################################

use_bpm 60
#use_bpm 10

############################################
# CONFIG
############################################

set :other_side, false

############################################
# SHARED HELPERS
############################################

define :memory_note do |n|
  synth :blade,
    note: n,
    release: 2,
    amp: 0.45
end

define :particle_note do |n|
  synth :pretty_bell,
    note: n,
    release: rrand(1.5,3),
    amp: 0.35
end

############################################
# CHANNEL 1
# MEMORY DRONE
############################################

define :memory_drone_normal do
  
  with_fx :reverb,
    room: 1,
  mix: 0.8 do
    
    synth :hollow,
      note: :e2,
      attack: 4,
      sustain: 8,
      release: 8,
      amp: 0.45
  end
  
  sleep 16
end

############################################
# CHANNEL 2
# FATHER THEME
############################################

define :father_theme_normal do
  
  use_synth :blade
  
  with_fx :reverb,
    room: 0.9,
  mix: 0.7 do
    
    memory_note :e3
    sleep 2
    
    memory_note :g3
    sleep 2
    
    memory_note :b3
    sleep 2
    
    memory_note :a3
    sleep 4
  end
end

############################################
# CHANNEL 3
# BROKEN PULSE
############################################

define :broken_pulse_normal do
  
  sample :bd_haus,
    amp: 0.5,
    rate: 0.35
  
  sleep 8
  
  sample :elec_blip,
    amp: 0.32,
    rate: 0.25
  
  sleep 8
end

############################################
# CHANNEL 4
# MEMORY PARTICLES
############################################

define :memory_particles_normal do
  
  sleep [4,8,12].choose
  
  with_fx :echo,
    phase: 0.75,
  decay: 4 do
    
    particle_note(
      [:e4,:g4,:b4,:d5].choose
    )
  end
end

############################################
# CHANNEL 5
# LIMINAL ATMOSPHERE
############################################

define :liminal_atmosphere_normal do
  
  with_fx :hpf,
  cutoff: 70 do
    
    synth :noise,
      attack: 3,
      sustain: 4,
      release: 4,
      amp: 0.24
  end
end

############################################
# CHANNEL 6
# MEMORY FRAGMENTS
#
# Recuerdos desconectados.
# Aparece poco.
#
############################################

define :memory_fragments_normal do
  
  sleep [8,16,24].choose
  
  use_synth :dark_ambience
  
  play [:e4,:g4,:a4,:b4].choose,
    release: 6,
    amp: 0.32
end

############################################
# CHANNEL 1
############################################

live_loop :channel_memory_drone do
  memory_drone_normal
end

############################################
# CHANNEL 2
############################################

live_loop :channel_father_theme do
  father_theme_normal
end

############################################
# CHANNEL 3
############################################

live_loop :channel_broken_pulse do
  broken_pulse_normal
end

############################################
# CHANNEL 4
############################################

live_loop :channel_particles do
  memory_particles_normal
end

############################################
# CHANNEL 5
############################################

live_loop :channel_atmosphere do
  liminal_atmosphere_normal
  sleep 12
end

############################################
# CHANNEL 6
############################################

live_loop :channel_fragments do
  memory_fragments_normal
  sleep 4
end
