data = open('day2.txt', 'r').readlines()

csum = 0
for l in data:
    n = [int(d) for d in l.split()]
    csum += max(n) - min(n)
print('Outcome task#1:', csum)


def find_divisible(n):
    for i in range(len(n)):
        for j in range(i+1, len(n)):
            if n[i] % n[j] == 0:
                return n[i] // n[j]
    return 0


csum = 0
for l in data:
    csum += find_divisible(sorted([int(d) for d in l.split()], reverse=True))
print('Outcome task#2:', csum)
