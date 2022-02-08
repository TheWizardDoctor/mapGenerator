import matplotlib.colors
import networkx as nx
import matplotlib.pyplot as plt

G = nx.Graph()




# elevation is 100 meters (aka 60 == 6000m)
# precipitation is in average cm of rainfall per year
G.add_nodes_from(range(2500), biome='', elevation=-1, precipitation=0, latitude=0)

for j in range(50):
    for i in range(49):
        G.add_edge(i + j * 50, i + j * 50 + 1)
for j in range(49):
    for i in range(50):
        G.add_edge(i + j * 50, i + (j + 1) * 50)

colorMap = {}
nodes = list(G.nodes.data())



for n in nodes:
    temperature = (((n[1]['elevation'] - 60) / -2) + ((n[1]['precipitation'] - 90) / -3)) / 2
    if n[1]['elevation'] < 0:
        nx.set_node_attributes(G, {n[0]: {'biome': 'ocean'}})
        colorMap.update({n[0]: 0})
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

# pos is a dictionary with positions for the visualization of the node graph
pos = {}
for j in range(51):
    for i in range(51):
        pos[i + j * 50] = (i, j)

plt.figure(figsize=(12, 12))
nx.draw(G, pos,
        node_shape='s',
        node_size=200,
        edge_color='w',
        font_color='w',
        node_color=colors,
        vmin=0,
        vmax=8,
        cmap=matplotlib.colors.ListedColormap(['royalblue', 'powderblue', 'olive', 'yellowgreen', 'lightgreen',
                                               'forestgreen', 'gold', 'goldenrod', 'springgreen'])
        )
plt.show()
