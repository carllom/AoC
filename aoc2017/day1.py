data = open('day1.txt', 'r').readline()
print('Outcome task#1:', sum([int(data[i]) for i in range(len(data)) if data[i] == data[(i+1) % len(data)]]))
print('Outcome task#2:', sum([int(data[i]) for i in range(len(data)) if data[i] == data[(i+len(data)//2) % len(data)]]))
