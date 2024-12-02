data = [list(map(int, r.split())) for r in open('day2-1.data').read().splitlines()]


def report(q):
    return [q[i] - q[i+1] for i in range(len(q)-1)]  # difference between each element


def checkreport(q):
    r = report(q)
    s = map(lambda x: x > 3 or x < -3 or x == 0, r)  # difference < 4 and not 0

    if any(s):
        return False

    if len(list(filter(lambda x: x > 0, r))) != len(r) and len(list(filter(lambda x: x < 0, r))) != len(r):  # either all positive or all negative
        return False

    return True


safe = 0
for q in data:
    if checkreport(q):
        safe += 1
        continue

print('Day2-1', safe)

safe = 0
for q in data:
    if checkreport(q):
        safe += 1
        continue  # skip if already safe
    for i in range(len(q)):  # remove an element and retry
        r = q.copy()
        r.pop(i)
        if checkreport(r):
            safe += 1
            break  # passed with removed element @ i

print('Day2-2', safe)
