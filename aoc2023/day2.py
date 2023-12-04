
s=0
with open('day2-1.data') as input:
    for line in input:
        cubz = {
            'red':12,
            'green':13,
            'blue':14
        }
        game_ok = True
        game = line.split(':')
        for round in game[1].split(';'):
            for col in round.split(','):
                n,c = col.strip().split(' ')
                if int(n) > cubz[c]:
                    game_ok = False
                    break
            if not game_ok:
                break
        if game_ok:
            s+=int(game[0].split()[1])
print('Day2-1:',s)

s=0
with open('day2-1.data') as input:
    for line in input:
        cubz = {
            'red':0,
            'green':0,
            'blue':0
        }
        game_ok = True
        game = line.split(':')
        for round in game[1].split(';'):
            for col in round.split(','):
                n,c = col.strip().split(' ')
                cubz[c] = max(cubz[c],int(n))
        s+= cubz['red'] * cubz['green'] * cubz['blue']
print('Day2-1:',s)