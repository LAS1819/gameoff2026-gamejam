# REMEMBER SONG

## CONFIGURATION
use_bpm 60
set :bar_length, 4

################

## DEFINITIONS

### Metronome
define :metronome do
  play :e2, amp: 0.5
end

### Remember Bass V1
define :remember_bass do
  with_synth :bass_foundation do
    
    play :e1,
      attack: 0.1,
      sustain: 3.5,
      release: 1.5,
      cutoff: 70,
      amp: 1
  end
  
end

### Remember Bass V2
define :remember_bass_v2 do
    with_fx :reverb,
    room: 0.7,
    mix: 0.3 do
      with_synth :bass_foundation do
        play_chord [:e1, :b1],
          attack: 0.05,
          sustain: 2.5,
          release: 2,
          cutoff: 60,
          amp: 1
      end
    end
end


################

## LIVE LOOPS

### CHANEL 0 ###
live_loop :metronome do
  stop
  metronome
  sleep 1
end

### CHANEL 1 ###
live_loop :bass do
  #stop
  remember_bass
  sleep get(:bar_length) * 2
end


################