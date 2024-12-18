import heapq


def astar(mem, start, goal):
    neighbors = [(0, 1), (1, 0), (0, -1), (-1, 0)]  # down, right, up, left
    queue, visited, came_from = [], set(), {}
    f, g = {start: (70, 70)}, {start: 0}

    heapq.heappush(queue, (f[start], start))

    while queue:
        current = heapq.heappop(queue)[1]
        if current == goal:
            path = []
            while current in came_from:  # reconstruct path
                path.append(current)
                current = came_from[current]
            return path

        visited.add(current)
        for i, j in neighbors:
            neighbor = current[0] + i, current[1] + j
            new_g = g[current] + 1

            if not (0 <= neighbor[0] < len(mem[0]) and 0 <= neighbor[1] < len(mem)) or mem[neighbor[1]][neighbor[0]] == '#':
                continue  # out of bounds or hit an obstacle

            if neighbor in visited and new_g >= g.get(neighbor, 0):
                continue  # already visited with a better path

            if new_g < g.get(neighbor, 0) or neighbor not in [i[1] for i in queue]:
                came_from[neighbor] = current
                g[neighbor] = new_g
                f[neighbor] = new_g + 70 - neighbor[0] + 70 - neighbor[1]  # g + h. h is manhattan distance to goal(at 70,70)
                heapq.heappush(queue, (f[neighbor], neighbor))
    return []  # no path found


bytes = [list(map(int, p.split(','))) for p in open('day18.data').read().splitlines()]  # corrupt byte locations
mem = [['.']*71 for _ in range(71)]  # 71x71 grid (the memory)
for i in range(1024):  # populate the memory with the first 1024 corrupt bytes
    mem[bytes[i][1]][bytes[i][0]] = '#'  # drop byte (obstacle)
path = set(astar(mem, (0, 0), (70, 70)))  # a set makes path search faster in task 2 - we do not rely on path order
print('Day18-1', len(path))

for i in range(1024, len(bytes)):  # continue to place corrupt bytes
    mem[bytes[i][1]][bytes[i][0]] = '#'
    if (bytes[i][0], bytes[i][1]) not in path:
        continue  # Optimize - if the byte is not in the current path, skip
    path = set(astar(mem, (0, 0), (70, 70)))  # recalculate path
    if not path:  # cannot reach the goal
        print('Day18-2', ','.join(map(str, bytes[i])))
        quit()
