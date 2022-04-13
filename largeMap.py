import matplotlib.colors
import networkx as nx
import numpy.random as random
import matplotlib.pyplot as plt

G = nx.Graph()

# elevation is 100 meters (aka 60 == 6000m)
# precipitation is in average cm of rainfall per year
G.add_nodes_from(range(20000), biome='', elevation=-1, precipitation=0, latitude=0)

for j in range(100):
    for i in range(199):
        G.add_edge(i + j * 200, i + j * 200 + 1)
for j in range(99):
    for i in range(200):
        G.add_edge(i + j * 200, i + (j + 1) * 200)

colorMap = {}
nodes = list(G.nodes.data())

elevationShape = 2
elevationScale = 120
precipitationShape = 3
precipitationScale = 75
lat = -91.8

# pos is a dictionary with positions for the visualization of the node graph
pos = {}
for j in range(100):
    for i in range(200):
        pos[i + j * 200] = (i, j)

for n in nodes:
    if n[0] % 200 == 0:
        lat += 1.8
    nx.set_node_attributes(G, {n[0]: {'precipitation': random.default_rng().gamma(precipitationShape, precipitationScale), 'latitude': lat}})
# 'elevation': random.default_rng().gamma(elevationShape, elevationScale) - 100,

for i in range(4):
    mPos = [random.randint(50, 150), random.randint(25, 75)]
    mNode = mPos[0] + mPos[1] * 200
    nx.set_node_attributes(G, {mNode: {'elevation': 450}})
    eligibleNeighbors = []
    for j in range(random.randint(20, 30)):
        eligibleNeighbors = []
        for n in G.neighbors(mNode):
            if nodes[n][1]['elevation'] < 0:
                eligibleNeighbors.append(n)
        mNext = 0
        while len(eligibleNeighbors) != 0:
            mNext = random.randint(0, len(eligibleNeighbors))
            potentialNeighbors = G.neighbors(eligibleNeighbors[mNext])
            flag = 0
            for n in potentialNeighbors:
                if nodes[n][1]['elevation'] > 0:
                    flag += 1
            if flag < 2:
                break
            else:
                eligibleNeighbors.pop(mNext)
        if len(eligibleNeighbors) == 0:
            break
        nx.set_node_attributes(G, {eligibleNeighbors[mNext]: {'elevation': 450}})
        mNode = eligibleNeighbors[mNext]



for n in nodes:
    temperature = (((n[1]['elevation'] * -0.1 + 40) + (abs(n[1]['latitude']) * -.5 + 30) * 2) / 3)
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
    # if n[1]['biome'] != 'ocean':
        # print(str(n[0]) + "\t" + n[1]['biome'] + "\t" + str(temperature))
        # print(n[1]['elevation'] * -0.075 + 30)
        # print(n[1]['latitude'] * -.5 + 30)

colors = [colorMap.get(node, 0) for node in G.nodes()]

plt.figure(figsize=(24, 12))
nx.draw(G, pos,
        node_shape='s',
        node_size=50,
        edge_color='w',
        font_color='w',
        node_color=colors,
        vmin=0,
        vmax=9,
        cmap=matplotlib.colors.ListedColormap(['royalblue', 'powderblue', 'olive', 'yellowgreen', 'lightgreen',
                                               'forestgreen', 'gold', 'goldenrod', 'springgreen', 'slategray'])
        )
plt.show()
