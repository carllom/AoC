map = dict([(int(l.split()[0]), [int(m) for m in l.split('<->')[1].split(',')]) for l in open('day12.txt').read().splitlines()])

visited = set()


def dfs(node):
    if node in visited:
        return
    visited.add(node)
    for n in map[node]:
        dfs(n)


dfs(0)

print('Outcome task#1', len(visited))

num_groups = 1  # 0-group is already visited
not_visited = set(map.keys()) - visited

while not_visited:
    dfs(not_visited.pop())
    num_groups += 1
    not_visited = set(map.keys()) - visited

print('Outcome task#2', num_groups)
