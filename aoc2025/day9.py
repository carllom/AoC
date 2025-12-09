data = [tuple(map(int, x.split(','))) for x in open('day9-1.data', encoding='utf-8')]


def find_all_rectangles(data):  # find all rectangles for two points in the data set sorted by area descending
    rectangles = []
    for i, (x1, y1) in enumerate(data[:-1]):
        for (x2, y2) in data[i + 1:]:
            area = (abs(x2 - x1)+1) * (abs(y2 - y1)+1)
            rectangles.append((area, ((min(x1, x2), min(y1, y2)), (max(x1, x2), max(y1, y2)))))
    rectangles.sort(reverse=True, key=lambda x: x[0])
    return rectangles


rectangles = find_all_rectangles(data)
print('Day9-1:', rectangles[0][0])

lines = []  # make a list of all horizontal and vertical lines for two subsequent points in the data set
for i in range(len(data)-1):
    (x1, y1) = data[i]
    (x2, y2) = data[i+1]
    lines.append(((min(x1, x2), min(y1, y2)), (max(x1, x2), max(y1, y2)), 'h' if y1 == y2 else 'v'))
lines.append(((min(data[-1][0], data[0][0]), min(data[-1][1], data[0][1])), (max(data[-1][0], data[0][0]),
             max(data[-1][1], data[0][1])), 'h' if data[0][1] == data[-1][1] else 'v'))

for (area, ((x1, y1), (x2, y2))) in rectangles:
    crossed = False  # true if any line crosses the rectangle
    for (l1, l2, lt) in lines:
        if lt == 'h':  # horizontal line
            if y1 < l1[1] < y2 and not (l2[0] <= x1 or l1[0] >= x2):
                crossed = True
                break
        else:  # vertical line
            if x1 < l1[0] < x2 and not (l2[1] <= y1 or l1[1] >= y2):
                crossed = True
                break
    if not crossed:
        print('Day9-2:', area)
        break
