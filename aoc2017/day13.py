def severity(layers, delay=0):
    return sum([d * r for d, r in layers.items() if (d + delay) % (r * 2 - 2) == 0])


layers = dict([(int(l.split(':')[0]), int(l.split(':')[1])) for l in open('day13.txt').readlines()])
print('Outcome part#1', severity(layers))
delay = 0
while True:
    if severity(layers, delay) == 0 and delay % (layers[0] * 2 - 2) != 0:  # 0 must not be caught in 0 (gives severity 0)
        break
    delay += 1
print('Outcome part#2', delay)
