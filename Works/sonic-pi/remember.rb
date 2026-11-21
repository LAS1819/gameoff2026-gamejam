# REMEMBER SONG

######################################
## REMEMBER SONG CONFIGURATION
######################################

use_bpm 80
set :bar_length, 4

######################################
## TIMELINE
######################################

live_loop :timeline do
  current_bar = tick - 1
  set :song_bar, current_bar
  puts "TIMELINE => #{current_bar}"
  cue :bar
  sleep get(:bar_length)
end

######################################
## LIVE LOOPS
######################################

### CHANEL 0 ###
live_loop :metronome do
  stop
  metronome
  sleep 1
end

### CHANEL 1 ###
live_loop :bass do
  #stop
  puts "[BASS] START"
  #remember_bass
  #sleep get(:bar_length) * 2
  sync :bar
  puts "[BASS] BAR => #{get(:song_bar)}"
  current_bar = get(:song_bar)
  next if current_bar < 8
  next unless current_bar.even?
  remember_bass_v2
end
################

### CHANEL 2 ###
live_loop :piano do
  #stop
  puts "[PIANO] START"
  #sleep get(:bar_length) * 8
  #remember_piano_v1
  sync :bar
  puts "[PIANO] BAR => #{get(:song_bar)}"
  remember_piano_v2
  #sleep get(:bar_length)
end

### CHANEL 3 ###
live_loop :piano_2 do
  puts "[PIANO_2] START"
  sync :bar
  puts "[PIANO_2] BAR => #{get(:song_bar)}"
  current_bar = get(:song_bar)
  next if current_bar < 4
  
  patterns = [
    remember_theme_a,
    remember_theme_a,
    remember_theme_b,
    remember_theme_b
  ]
  
  pattern_index = ((current_bar - 4) / 2) % patterns.length
  remember_piano_v1 patterns[pattern_index]
  
end

live_loop :piano_transpose do
  stop
  remember_piano_v3 0, 0.8
  sleep get(:bar_length)
  remember_piano_v3 -2, 0.8
  sleep get(:bar_length)
  remember_piano_v3 0, 1
  sleep get(:bar_length)
  remember_piano_v3 3, 0.7
  sleep get(:bar_length)
end

### CHANEL 4 ###
live_loop :piano_transpose_2 do
  stop
  [0,-2,-5,-2].each do |t|
    remember_piano_v3 t, 0.8
    sleep get(:bar_length)
  end
  
end
################

### CHANEL 5 ###
live_loop :piano_3 do

  sync :bar
  puts "[PIANO_3] START"
  current_bar = get(:song_bar)
  next if current_bar < 8
  next unless current_bar % 4 == 0
  puts "[PIANO_3] BAR => #{current_bar}"

  remember_piano_v4

end