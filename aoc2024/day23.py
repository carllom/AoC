import networkx as nx

G = nx.Graph()
for c in [x.split('-') for x in open('day23.data').read().splitlines()]:
    G.add_edge(c[0], c[1])

cliques = list(nx.enumerate_all_cliques(G))
print('Day23-1', len(list(filter(lambda x: len(x) == 3 and any(map(lambda a: a.startswith('t'),x)), cliques))))
print('Day23-2',','.join(sorted(sorted(cliques, key=lambda x: len(x), reverse=True)[0])))
