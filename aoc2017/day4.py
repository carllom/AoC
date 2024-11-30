
valid = 0
with open('day4.txt', 'r') as input:
    for phrase in input.readlines():
        words = phrase.split()
        if len(words) == len(set(words)):
            valid += 1
print('Outcome task#1:', valid)

valid = 0
with open('day4.txt', 'r') as input:
    for phrase in input.readlines():
        words = [''.join(sorted(word)) for word in phrase.split()]
        if len(words) == len(set(words)):
            valid += 1
print('Outcome task#2:', valid)
