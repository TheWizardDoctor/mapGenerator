import matplotlib.colors
import networkx as nx
import matplotlib.pyplot as plt

G = nx.Graph()

b = 'biome'
e = 'elevation'
p = 'precipitation'
l = 'latitude'
tempMap = [( 0, {b: '', e: -1, p: 0, l: 0}), ( 1, {b: '', e: -1, p: 0, l: 0}), ( 2, {b: '', e: -1, p: 0, l: 0}), ( 3, {b: '', e: -1, p: 0, l: 0}), ( 4, {b: '', e: -1, p: 0, l: 0}), ( 5, {b: '', e: -1, p: 0, l: 0}), ( 6, {b: '', e: -1, p: 0, l: 0}), ( 7, {b: '', e: -1, p: 0, l: 0}), ( 8, {b: '', e: -1, p: 0, l: 0}), ( 9, {b: '', e: -1, p: 0, l: 0}),
           (10, {b: '', e: -1, p: 0, l: 0}), (11, {b: '', e: -1, p: 0, l: 0}), (12, {b: '', e: 1, p: 0, l: 0}), (13, {b: '', e: 1, p: 0, l: 0}), (14, {b: '', e: -1, p: 0, l: 0}), (15, {b: '', e: -1, p: 0, l: 0}), (16, {b: '', e: 1, p: 220, l: 0}), (17, {b: '', e: 1, p: 220, l: 0}), (18, {b: '', e: -1, p: 0, l: 0}), (19, {b: '', e: -1, p: 0, l: 0}),
           (20, {b: '', e: -1, p: 0, l: 0}), (21, {b: '', e: 1, p: 0, l: 0}), (22, {b: '', e: 1, p: 0, l: 0}), (23, {b: '', e: 1, p: 0, l: 0}), (24, {b: '', e: 1, p: 110, l: 0}), (25, {b: '', e: 1, p: 220, l: 0}), (26, {b: '', e: 1, p: 220, l: 0}), (27, {b: '', e: 1, p: 220, l: 0}), (28, {b: '', e: -1, p: 0, l: 0}), (29, {b: '', e: -1, p: 0, l: 0}),
           (30, {b: '', e: -1, p: 0, l: 0}), (31, {b: '', e: 1, p: 0, l: 0}), (32, {b: '', e: 1, p: 110, l: 0}), (33, {b: '', e: 1, p: 110, l: 0}), (34, {b: '', e: 200, p: 0, l: 0}), (35, {b: '', e: 200, p: 110, l: 0}), (36, {b: '', e: 200, p: 220, l: 0}), (37, {b: '', e: 2, p: 220, l: 0}), (38, {b: '', e: 2, p: 220, l: 0}), (39, {b: '', e: -1, p: 0, l: 0}),
           (40, {b: '', e: -1, p: 0, l: 0}), (41, {b: '', e: -1, p: 0, l: 0}), (42, {b: '', e: 100, p: 0, l: 0}), (43, {b: '', e: 1, p: 0, l: 0}), (44, {b: '', e: 1, p: 0, l: 0}), (45, {b: '', e: 1, p: 220, l: 0}), (46, {b: '', e: 1, p: 220, l: 0}), (47, {b: '', e: 1, p: 220, l: 0}), (48, {b: '', e: 1, p: 220, l: 0}), (49, {b: '', e: -1, p: 0, l: 0}),
           (50, {b: '', e: -1, p: 0, l: 0}), (51, {b: '', e: 1, p: 101, l: 0}), (52, {b: '', e: 1, p: 101, l: 0}), (53, {b: '', e: 1, p: 110, l: 0}), (54, {b: '', e: 1, p: 220, l: 0}), (55, {b: '', e: 1, p: 220, l: 0}), (56, {b: '', e: 1, p: 220, l: 0}), (57, {b: '', e: 1, p: 220, l: 0}), (58, {b: '', e: -1, p: 0, l: 0}), (59, {b: '', e: -1, p: 0, l: 0}),
           (60, {b: '', e: -1, p: 0, l: 0}), (61, {b: '', e: -1, p: 0, l: 0}), (62, {b: '', e: 1, p: 201, l: 0}), (63, {b: '', e: 1, p: 201, l: 0}), (64, {b: '', e: 1, p: 201, l: 0}), (65, {b: '', e: 100, p: 201, l: 0}), (66, {b: '', e: 100, p: 201, l: 0}), (67, {b: '', e: 100, p: 201, l: 0}), (68, {b: '', e: -1, p: 0, l: 0}), (69, {b: '', e: -1, p: 0, l: 0}),
           (70, {b: '', e: -1, p: 0, l: 0}), (71, {b: '', e: -1, p: 0, l: 0}), (72, {b: '', e: -1, p: 0, l: 0}), (73, {b: '', e: 1, p: 101, l: 0}), (74, {b: '', e: 1, p: 0, l: 0}), (75, {b: '', e: 1, p: 0, l: 0}), (76, {b: '', e: 1, p: 101, l: 0}), (77, {b: '', e: 1, p: 101, l: 0}), (78, {b: '', e: -1, p: 0, l: 0}), (79, {b: '', e: -1, p: 0, l: 0}),
           (80, {b: '', e: -1, p: 0, l: 0}), (81, {b: '', e: -1, p: 0, l: 0}), (82, {b: '', e: -1, p: 0, l: 0}), (83, {b: '', e: -1, p: 0, l: 0}), (84, {b: '', e: -1, p: 0, l: 0}), (85, {b: '', e: 1, p: 0, l: 0}), (86, {b: '', e: 1, p: 0, l: 0}), (87, {b: '', e: -1, p: 0, l: 0}), (88, {b: '', e: -1, p: 0, l: 0}), (89, {b: '', e: -1, p: 0, l: 0}),
           (90, {b: '', e: -1, p: 0, l: 0}), (91, {b: '', e: -1, p: 0, l: 0}), (92, {b: '', e: -1, p: 0, l: 0}), (93, {b: '', e: -1, p: 0, l: 0}), (94, {b: '', e: -1, p: 0, l: 0}), (95, {b: '', e: -1, p: 0, l: 0}), (96, {b: '', e: -1, p: 0, l: 0}), (97, {b: '', e: -1, p: 0, l: 0}), (98, {b: '', e: -1, p: 0, l: 0}), (99, {b: '', e: -1, p: 0, l: 0})
           ]


# elevation is 10 meters (aka 60 == 600m)
# precipitation is in average cm of rainfall per year
# G.add_nodes_from(range(100), biome='', elevation=-1, precipitation=0, latitude=0)
G.add_nodes_from(tempMap)

for j in range(10):
    for i in range(9):
        G.add_edge(i + j * 10, i + j * 10 + 1)
for j in range(9):
    for i in range(10):
        G.add_edge(i + j * 10, i + (j + 1) * 10)

# pos is a dictionary with positions for the visualization of the node graph
pos = {}
for j in range(11):
    for i in range(11):
        pos[i + j * 10] = (i, j)

colorMap = {}
nodes = list(G.nodes.data())

for n in nodes:
    nx.set_node_attributes(G, {n[0]: {'latitude': pos[n[0]][1] * 10}})

for n in nodes:
    temperature = (((n[1]['elevation'] * -0.075 + 30) + (n[1]['latitude'] * -.67 + 40) * 4) / 5)
    if n[1]['elevation'] < 0:
        nx.set_node_attributes(G, {n[0]: {'biome': 'ocean'}})
        colorMap.update({n[0]: 0})
    elif n[1]['elevation'] > 350:
        nx.set_node_attributes(G, {n[0]: {'biome': 'mountain'}})
        colorMap.update({n[0]: 9})
    elif temperature <= 5:
        if n[1]['precipitation'] <= 100:
            nx.set_node_attributes(G, {n[0]: {'biome': 'tundra'}})
            colorMap.update({n[0]: 1})
        else:
            nx.set_node_attributes(G, {n[0]: {'biome': 'borealForest'}})
            colorMap.update({n[0]: 2})
    elif temperature <= 20:
        if n[1]['precipitation'] <= 100:
            nx.set_node_attributes(G, {n[0]: {'biome': 'prairie'}})
            colorMap.update({n[0]: 3})
        elif n[1]['precipitation'] <= 200:
            nx.set_node_attributes(G, {n[0]: {'biome': 'shrubland'}})
            colorMap.update({n[0]: 4})
        else:
            nx.set_node_attributes(G, {n[0]: {'biome': 'temperateForest'}})
            colorMap.update({n[0]: 5})
    else:
        if n[1]['precipitation'] <= 100:
            nx.set_node_attributes(G, {n[0]: {'biome': 'desert'}})
            colorMap.update({n[0]: 6})
        elif n[1]['precipitation'] <= 200:
            nx.set_node_attributes(G, {n[0]: {'biome': 'savannah'}})
            colorMap.update({n[0]: 7})
        else:
            nx.set_node_attributes(G, {n[0]: {'biome': 'rainforest'}})
            colorMap.update({n[0]: 8})


colors = [colorMap.get(node, 0) for node in G.nodes()]

plt.figure(figsize=(10, 10))
nx.draw(G, pos,
        with_labels=True,
        node_shape='s',
        node_size=4000,
        edge_color='w',
        font_color='w',
        node_color=colors,
        vmin=0,
        vmax=9,
        cmap=matplotlib.colors.ListedColormap(['royalblue', 'powderblue', 'olive', 'yellowgreen', 'lightgreen',
                                               'forestgreen', 'gold', 'goldenrod', 'springgreen', 'slategray'])
        )
plt.show()

"""
for i in range(0, 900):
    if i % 30 == 0:
        print()
    print("(",end='')
    if i < 10:
        print(" ", end='')
    if i < 100:
        print(" ", end='')
    print(str(i) + ", {b: '', e: -1, p: 0, l: 0}), ", end='')
"""
