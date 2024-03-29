# New idea - use patterns for each list item

1 = #.
2 = ##.
..and so on

slide these across the string and record all possible starting points

you will end up with a list of starting points for each item
[
(1, [0,3,4,5,6,12])
(1, [0,3,4,5,6,12])
(3, [5])
]

then you can probably sort and cut down the starting points to a subset
and do a product of the length of all starting point lists

Comb: 10 ('?###????????', [3, 2, 1])

[(3, [1, 2, 3, 4, 5]),
 (2, [5, 6, 7, 8, 9]),
 (1, [8, 9, 10, 11])]

 [(3, [1]),
  (2, [5, 6, 7, 8, 9]),
  (1, [8, 9, 10, 11])]

 1 => 1+(3+1)=5 => 5+(2+1)=8 => 8..11 = 4
                   6+(2+1)=9 => 9..11 = 3
                   7+(2+1)=10 => 10..11 = 2
                   8+(2+1)=11 => 11.11 = 1

Comb: 16384 ('.??..??...?##.', [1, 1, 3])
[(1, [1, 2, 5, 6]),
 (1, [5, 6]),
 (3, [10, 25, 40]),
 (1, [14, 16, 17, 20, 21]),
 (1, [16, 17, 20, 21]),
 (3, [25, 40]),
 (1, [29, 31, 32, 35, 36]),
 (1, [31, 32, 35, 36]),
 (3, [40, 55]),
 (1, [44, 46, 47, 50, 51]),
 (1, [46, 47, 50, 51]),
 (3, [55]),
 (1, [59, 61, 62, 65, 66]),
 (1, [61, 62, 65, 66]),
 (3, [70])]

 remove impossible (last element >= last element in next row):
 [(1, [1, 2, 5]),
 (1, [5, 6]),
 (3, [10]),
 (1, [14, 16, 17, 20]),
 (1, [16, 17, 20, 21]),
 (3, [25]),
 (1, [29, 31, 32, 35]),
 (1, [31, 32, 35, 36]),
 (3, [40]),
 (1, [44, 46, 47, 50]),
 (1, [46, 47, 50, 51]),
 (3, [55]),
 (1, [59, 61, 62, 65]),
 (1, [61, 62, 65, 66]),
 (3, [70])]

 remove impossible (last element will overlap(including length) with last element in next row):
 [(1, [1, 2]),
 (1, [5, 6]),
 (3, [10]),
 (1, [14, 16, 17]),
 (1, [16, 17, 20, 21]),
 (3, [25]),
 (1, [29, 31, 32]),
 (1, [31, 32, 35, 36]),
 (3, [40]),
 (1, [44, 46, 47]),
 (1, [46, 47, 50, 51]),
 (3, [55]),
 (1, [59, 61, 62]),
 (1, [61, 62, 65, 66]),
 (3, [70])]

 4*3 - all less or equal to item - segment length
 59-61 62 65 66
 61-65 66
 62-65 66

 8st

1
[(1, [1]),
 (3, [3]),
 (1, [7]),
 (6, [9, 10]),
 (1, [17]),
 (3, [19]),
 (1, [23]),
 (6, [25, 26]),
 (1, [33]),
 (3, [35]),
 (1, [39]),
 (6, [41, 42]),
 (1, [49]),
 (3, [51]),
 (1, [55]),
 (6, [57, 58]),
 (1, [65]),
 (3, [67]),
 (1, [71]),
 (6, [73])]

Comb: 16 ('????.#...#...', [4, 1, 1]) [(4, [0, 13, 14]), (1, [5]), (1, [9]), (4, [13, 14, 27, 28]), (1, [19]), (1,
[23]), (4, [27, 28, 41, 42]), (1, [33]), (1, [37]), (4, [41, 42]), (1, [47]), (1, [51]), (4, [55, 56]), (1, [61]),
(1, [65])]

Comb: 2500 ('????.######..#####.', [1, 6, 5]) [(1, [0, 1, 2, 3]), (6, [5]), (5, [13]), (1, [19, 20, 21, 22, 23]), (6, [25]), (5, [33]), (1, [39, 40, 41, 42, 43]), (6, [45]), (5, [53]), (1, [59, 60, 61, 62, 63]), (6, [65]), (5, [73]), (1, [79, 80, 81, 82, 83]), (6, [85]), (5, [93])]

almost there - we still cannot skip a place with a hash sign. Those combinations are illegal

???.###????.###????.###????.###????.### [1, 1, 3, 1, 1, 3, 1, 1, 3, 1, 1, 3, 1, 1, 3]
Comb: 1 ('???.###', [1, 1, 3]) [
  (1, [0, 1]),
  (1, [2]),
  (3, [4]),
  (1, [8, 9]),
  (1, [10]),
  (3, [12]),
  (1, [16, 17]),
  (1, [18]),
  (3, [20]),
  (1, [24, 25]),
  (1, [26]),
  (3, [28]),
  (1, [32, 33]),
  (1, [34]),
  (3, [36])
  ]

1. remove impossible (last element >= last element in next row): - no change
2. remove impossible (last element will overlap(including length) with last element in next row):
  (1, [0]),
  (1, [2]),
  (3, [4]),
  (1, [8]),
  (1, [10]),
  (3, [12]),
  (1, [16]),
  (1, [18]),
  (3, [20]),
  (1, [24]),
  (1, [26]),
  (3, [28]),
  (1, [32]),
  (1, [34]),
  (3, [36])
1 combination

.??..??...?##.?.??..??...?##.?.??..??...?##.?.??..??...?##.?.??..??...?##. [1, 1, 3, 1, 1, 3, 1, 1, 3, 1, 1, 3, 1, 1, 3]
Comb: 16384 ('.??..??...?##.', [1, 1, 3]) [
  (1, [1, 2, 5, 6]),
  (1, [5, 6]),
  (3, [10, 25, 40]),
  (1, [14, 16, 17, 20, 21]),
  (1, [16, 17, 20, 21]),
  (3, [25, 40]),
  (1, [29, 31, 32, 35, 36]),
  (1, [31, 32, 35, 36]),
  (3, [40, 55]),
  (1, [44, 46, 47, 50, 51]),
  (1, [46, 47, 50, 51]),
  (3, [55]),
  (1, [59, 61, 62, 65, 66]),
  (1, [61, 62, 65, 66]),
  (3, [70])
  ]
1,2 above
  (1, [1, 2]),
  (1, [5, 6]),
  (3, [10]),
  (1, [14, 16, 17]),
  (1, [16, 17, 20, 21]),
  (3, [25]),
  (1, [29, 31, 32]),
  (1, [31, 32, 35, 36]),
  (3, [40]),
  (1, [44, 46, 47]),
  (1, [46, 47, 50, 51]),
  (3, [55]),
  (1, [59, 61, 62]),
  (1, [61, 62, 65, 66]),
  (3, [70])
3: remove all singles
  (1, [1, 2]),
  (1, [5, 6]),
  (1, [14, 16, 17]),
  (1, [16, 17, 20, 21]),
  (1, [29, 31, 32]),
  (1, [31, 32, 35, 36]),
  (1, [44, 46, 47]),
  (1, [46, 47, 50, 51]),
  (1, [59, 61, 62]),
  (1, [61, 62, 65, 66]),

multiply from "bottom up":
  (1, [
    59, 4 valid (61,62,65,66)
    61, 2 valid (65,66)
    62  2 valud (65,66)
    ]),
next:
  (1, [
    46, 8 valid (59,61,62)
    47, 8 valid (59,61,62)
    50, 8 valid (59,61,62)
    51  8 valid (59,61,62)
    ]),
  (1, [
    44, 32 valid (46,47,50,51)
    46, 16 valid (50,51)
    47  16 valid (50,51)
    ]),
  (1, [
    31, 64 valid (44,46,47)
    32, 64 valid (44,46,47)
    35, 64 valid (44,46,47)
    36  64 valid (44,46,47)
    ]),
  (1, [
    29, 256 valid (31,32,35,36)
    31, 128 valid (35,36)
    32  128 valid (35,36)
    ]),
  (1, [
    16, 512 valid (29,31,32)
    17, 512 valid (29,31,32)
    20, 512 valid (29,31,32)
    21  512 valid (29,31,32)
    ]),
  (1, [
    14, 2048 valid (16,17,20,21)
    16, 1024 valid (20,21)
    17  1024 valid (20,21)
    ]),
  (1, [
    5, 4096 valid (14,16,17)
    6  4096 valid (14,16,17)
    ]),
  (1, [
    1, 8192 valid (5,6)
    2  8192 valid (5,6)
    ]),
  = 16384!

  ????.#...#...?????.#...#...?????.#...#...?????.#...#...?????.#...#... [4, 1, 1, 4, 1, 1, 4, 1, 1, 4, 1, 1, 4, 1, 1]
  Comb: 16 ('????.#...#...', [4, 1, 1]) [
    (4, [0]),
    (1, [5]),
    (1, [9]),
    (4, [13, 14]),
    (1, [19]),
    (1, [23]),
    (4, [27, 28]),
    (1, [33]),
    (1, [37]),
    (4, [41, 42]),
    (1, [47]),
    (1, [51]),
    (4, [55, 56]),
    (1, [61]),
    (1, [65])
    ]

    (4, [55, 56]),
    (4, [
      41, 2 valid (55,56)
      42  2 valid (55,56)
      ]),
    (4, [
      27, 4 valid (41,42)
      28  4 valid (41,42)
      ]),
    (4, [
      13, 8 valid (27,28)
      14  8 valid (27,28)
      ]),
    = 16!

?#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#? [1, 3, 1, 6, 1, 3, 1, 6, 1, 3, 1, 6, 1, 3, 1, 6, 1, 3, 1, 6]
0
0 ?# #. False
1 #? #. True
3
3 #?#? ###. True
4 ?#?# ###. False
5 #?#? ###. True
6 ?#?# ###. False
7 #?#? ###. True
8 ?#?# ###. False
7
7 #? #. True
9
9 #?#?#?? ######. True
10 ?#?#??? ######. True
11 #?#???# ######. False
12 ?#???#? ######. True
13 #???#?# ######. False
14 ???#?#? ######. True
16
16 ?# #. False
17 #? #. True
19
19 #?#? ###. True
20 ?#?# ###. False
21 #?#? ###. True
22 ?#?# ###. False
23 #?#? ###. True
23
23 #? #. True
25
25 #?#?#?? ######. True
26 ?#?#??? ######. True
27 #?#???# ######. False
28 ?#???#? ######. True
29 #???#?# ######. False
32
32 ?# #. False
33 #? #. True
35
35 #?#? ###. True
36 ?#?# ###. False
37 #?#? ###. True
38 ?#?# ###. False
39
39 #? #. True
41
41 #?#?#?? ######. True
42 ?#?#??? ######. True
43 #?#???# ######. False
44 ?#???#? ######. True
48
48 ?# #. False
49 #? #. True
51
51 #?#? ###. True
52 ?#?# ###. False
53 #?#? ###. True
55
55 #? #. True
57
57 #?#?#?? ######. True
58 ?#?#??? ######. True
59 #?#???# ######. False
64
64 ?# #. False
65 #? #. True
67
67 #?#? ###. True
68 ?#?# ###. False
71
71 #? #. True
73
73 #?#?#?. ######. True
74 ?#?#?. ######. False
Before (1, deque([1])) (3, deque([3, 5, 7]))
After  (1, deque([1])) (3, deque([3, 5, 7]))

Before (3, deque([3, 5, 7])) (1, deque([7]))
After  (3, deque([3])) (1, deque([7]))

0000000000111111111122222222223333333333444444444455555555556666666666777777777
0123456789012345678901234567890123456789012345678901234567890123456789012345678
?#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#?
Before (1, deque([7])) (6, deque([9, 10, 12, 14]))
After  (1, deque([7])) (6, deque([9, 10, 12, 14]))

so, if there is a '#' between end of the previous and a possible starting index it is not a legal combination

Before (6, deque([9, 10, 12, 14])) (1, deque([17]))
After  (6, deque([9, 10])) (1, deque([17]))

Before (1, deque([17])) (3, deque([19, 21, 23]))
After  (1, deque([17])) (3, deque([19, 21, 23]))

Before (3, deque([19, 21, 23])) (1, deque([23]))
After  (3, deque([19])) (1, deque([23]))

Before (1, deque([23])) (6, deque([25, 26, 28]))
After  (1, deque([23])) (6, deque([25, 26, 28]))

Before (6, deque([25, 26, 28])) (1, deque([33]))
After  (6, deque([25, 26])) (1, deque([33]))

Before (1, deque([33])) (3, deque([35, 37]))
After  (1, deque([33])) (3, deque([35, 37]))

Before (3, deque([35, 37])) (1, deque([39]))
After  (3, deque([35])) (1, deque([39]))

Before (1, deque([39])) (6, deque([41, 42, 44]))
After  (1, deque([39])) (6, deque([41, 42, 44]))

Before (6, deque([41, 42, 44])) (1, deque([49]))
After  (6, deque([41, 42])) (1, deque([49]))

Before (1, deque([49])) (3, deque([51, 53]))
After  (1, deque([49])) (3, deque([51, 53]))

Before (3, deque([51, 53])) (1, deque([55]))
After  (3, deque([51])) (1, deque([55]))

Before (1, deque([55])) (6, deque([57, 58]))
After  (1, deque([55])) (6, deque([57, 58]))

Before (6, deque([57, 58])) (1, deque([65]))
After  (6, deque([57, 58])) (1, deque([65]))

Before (1, deque([65])) (3, deque([67]))
After  (1, deque([65])) (3, deque([67]))

Before (3, deque([67])) (1, deque([71]))
After  (3, deque([67])) (1, deque([71]))

Before (1, deque([71])) (6, deque([73]))
After  (1, deque([71])) (6, deque([73]))

[(6, deque([57, 58])), (6, deque([41, 42])), (6, deque([25, 26])), (6, deque([9, 10]))]
Comb: -1 16 ('?#?#?#?#?#?#?#?', [1, 3, 1, 6]) [(1, deque([1])), (3, deque([3])), (1, deque([7])), (6, deque([9, 10])), (1, deque([17])), (3, deque([19])), (1, deque([23])), (6, deque([25, 26])), (1, deque([33])), (3, deque([35])), (1, deque([39])), (6, deque([41, 42])), (1, deque([49])), (3, deque([51])), (1, deque([55])), (6, deque([57, 58])), (1, deque([65])), (3, deque([67])), (1, deque([71])), (6, deque([73]))]

0000000000111111111122222222223333333333444444444455555555556666
0123456789012345678901234567890123456789012345678901234567890123
?###??????????###??????????###??????????###??????????###???????? [3, 2, 1, 3, 2, 1, 3, 2, 1, 3, 2, 1, 3, 2, 1]
 ###.
     ##======.XXX=========.
        #====.XXX=========.X
              ###.
                  ##======.
                     #====.XXX
the 3-pattern has potential locations @10,14
the next ##.-pattern starts scanning @ 14 and finds an

0
0  ?### ###. False
1  ###? ###. True
5
5  ??? ##. True
6  ??? ##. True
7  ??? ##. True
8  ??? ##. True
9  ??? ##. True
10 ??? ##. True
11 ??? ##. True
12 ??# ##. False
13 ?## ##. False
14 ### ##. False
18 ??? ##. True
19 ??? ##. True
20 ??? ##. True
21 ??? ##. True
22 ??? ##. True
23 ??? ##. True
24 ??? ##. True
25 ??# ##. False
8
8 ?? #. True
9 ?? #. True
10 ?? #. True
11 ?? #. True
12 ?? #. True
13 ?# #. False
14 ## #. False
18 ?? #. True
19 ?? #. True
20 ?? #. True
21 ?? #. True
22 ?? #. True
23 ?? #. True
24 ?? #. True
25 ?? #. True
26 ?# #. False
27 ## #. False
10
10 ???? ###. True
11 ???# ###. False
12 ??## ###. False
13 ?### ###. False
14 ###? ###. True
14
14 ### ##. False
18 ??? ##. True
19 ??? ##. True
20 ??? ##. True
21 ??? ##. True
22 ??? ##. True
23 ??? ##. True
24 ??? ##. True
25 ??# ##. False
26 ?## ##. False
27 ### ##. False
31 ??? ##. True
32 ??? ##. True
33 ??? ##. True
34 ??? ##. True
21
21 ?? #. True
22 ?? #. True
23 ?? #. True
24 ?? #. True
25 ?? #. True
26 ?# #. False
27 ## #. False
31 ?? #. True
32 ?? #. True
33 ?? #. True
34 ?? #. True
35 ?? #. True
36 ?? #. True
37 ?? #. True
23
23 ???? ###. True
24 ???# ###. False
25 ??## ###. False
26 ?### ###. False
27 ###? ###. True
27
27 ### ##. False
31 ??? ##. True
32 ??? ##. True
33 ??? ##. True
34 ??? ##. True
35 ??? ##. True
36 ??? ##. True
37 ??? ##. True
38 ??# ##. False
39 ?## ##. False
40 ### ##. False
34
34 ?? #. True
35 ?? #. True
36 ?? #. True
37 ?? #. True
38 ?? #. True
39 ?# #. False
40 ## #. False
44 ?? #. True
45 ?? #. True
46 ?? #. True
36
36 ???? ###. True
37 ???# ###. False
38 ??## ###. False
39 ?### ###. False
40 ###? ###. True
40
40 ### ##. False
44 ??? ##. True
45 ??? ##. True
46 ??? ##. True
47 ??? ##. True
48 ??? ##. True
49 ??? ##. True
50 ??? ##. True
51 ??# ##. False
52 ?## ##. False
47
47 ?? #. True
48 ?? #. True
49 ?? #. True
50 ?? #. True
51 ?? #. True
52 ?# #. False
53 ## #. False
49
49 ???? ###. True
50 ???# ###. False
51 ??## ###. False
52 ?### ###. False
53 ###? ###. True
53
53 ### ##. False
57 ??? ##. True
58 ??? ##. True
59 ??? ##. True
60 ??? ##. True
61 ??? ##. True
60
60 ?? #. True
61 ?? #. True
62 ?? #. True
63 ?. #. True
64 . #. False
Before (3, deque([1])) (2, deque([5, 6, 7, 8, 9, 10, 11, 18, 19, 20, 21, 22, 23, 24]))
After1 (3, deque([1])) (2, deque([5, 6, 7, 8, 9, 10, 11, 18, 19, 20, 21, 22, 23, 24]))

Before (2, deque([5, 6, 7, 8, 9, 10, 11, 18, 19, 20, 21, 22, 23, 24])) (1, deque([8, 9, 10, 11, 12, 18, 19, 20, 21, 22, 23, 24, 25]))
After1 (2, deque([5, 6, 7, 8, 9, 10, 11, 18, 19, 20, 21, 22])) (1, deque([8, 9, 10, 11, 12, 18, 19, 20, 21, 22, 23, 24, 25]))
After2 (2, deque([5, 6, 7, 8, 9, 10, 11])) (3, deque([1]))

Before (1, deque([8, 9, 10, 11, 12, 18, 19, 20, 21, 22, 23, 24, 25])) (3, deque([10, 14]))
After1 (1, deque([8, 9, 10, 11, 12])) (3, deque([10, 14]))
After2 (1, deque([8, 9, 10, 11, 12])) (2, deque([5, 6, 7, 8, 9, 10, 11]))

Before (3, deque([10, 14])) (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34]))
After1 (3, deque([10, 14])) (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34]))
After2 (3, deque([10, 14])) (1, deque([8, 9, 10, 11, 12]))

Before (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34])) (1, deque([21, 22, 23, 24, 25, 31, 32, 33, 34, 35, 36, 37]))
After1 (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34])) (1, deque([21, 22, 23, 24, 25, 31, 32, 33, 34, 35, 36, 37]))
After2 (2, deque([18, 19, 20, 21, 22, 23, 24])) (3, deque([10, 14]))

Before (1, deque([21, 22, 23, 24, 25, 31, 32, 33, 34, 35, 36, 37])) (3, deque([23, 27]))
After1 (1, deque([21, 22, 23, 24, 25])) (3, deque([23, 27]))
After2 (1, deque([21, 22, 23, 24, 25])) (2, deque([18, 19, 20, 21, 22, 23, 24]))

Before (3, deque([23, 27])) (2, deque([31, 32, 33, 34, 35, 36, 37]))
After1 (3, deque([23, 27])) (2, deque([31, 32, 33, 34, 35, 36, 37]))
After2 (3, deque([23, 27])) (1, deque([21, 22, 23, 24, 25]))

Before (2, deque([31, 32, 33, 34, 35, 36, 37])) (1, deque([34, 35, 36, 37, 38, 44, 45, 46]))
After1 (2, deque([31, 32, 33, 34, 35, 36, 37])) (1, deque([34, 35, 36, 37, 38, 44, 45, 46]))
After2 (2, deque([31, 32, 33, 34, 35, 36, 37])) (3, deque([23, 27]))

Before (1, deque([34, 35, 36, 37, 38, 44, 45, 46])) (3, deque([36, 40]))
After1 (1, deque([34, 35, 36, 37, 38])) (3, deque([36, 40]))
After2 (1, deque([34, 35, 36, 37, 38])) (2, deque([31, 32, 33, 34, 35, 36, 37]))

Before (3, deque([36, 40])) (2, deque([44, 45, 46, 47, 48, 49, 50]))
After1 (3, deque([36, 40])) (2, deque([44, 45, 46, 47, 48, 49, 50]))
After2 (3, deque([36, 40])) (1, deque([34, 35, 36, 37, 38]))

Before (2, deque([44, 45, 46, 47, 48, 49, 50])) (1, deque([47, 48, 49, 50, 51]))
After1 (2, deque([44, 45, 46, 47, 48])) (1, deque([47, 48, 49, 50, 51]))
After2 (2, deque([44, 45, 46, 47, 48])) (3, deque([36, 40]))

Before (1, deque([47, 48, 49, 50, 51])) (3, deque([49, 53]))
After1 (1, deque([47, 48, 49, 50, 51])) (3, deque([49, 53]))
After2 (1, deque([47, 48, 49, 50, 51])) (2, deque([44, 45, 46, 47, 48]))

Before (3, deque([49, 53])) (2, deque([57, 58, 59, 60, 61]))
After1 (3, deque([49, 53])) (2, deque([57, 58, 59, 60, 61]))
After2 (3, deque([49, 53])) (1, deque([47, 48, 49, 50, 51]))

Before (2, deque([57, 58, 59, 60, 61])) (1, deque([60, 61, 62, 63]))
After1 (2, deque([57, 58, 59, 60])) (1, deque([60, 61, 62, 63]))
After2 (2, deque([57, 58, 59, 60])) (3, deque([49, 53]))

After2 (1, deque([60, 61, 62, 63])) (2, deque([57, 58, 59, 60]))

0000000000111111111122222222223333333333444444444455555555556666
0123456789012345678901234567890123456789012345678901234567890123
?###??????????###??????????###??????????###??????????###???????? [3, 2, 1, 3, 2, 1, 3, 2, 1, 3, 2, 1, 3, 2, 1]

Before (3, deque([1, 5, 6, 7, 8, 9, 10, 14, 18, 19, 20, 21]))
Before (2, deque([5, 6, 7, 8, 9, 10, 11, 18, 19, 20, 21, 22, 23, 24]))
Before (1, deque([8, 9, 10, 11, 12, 18, 19, 20, 21, 22, 23, 24, 25]))
Before (3, deque([10, 14, 18, 19, 20, 21, 22, 23, 27]))
Before (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34]))
Before (1, deque([21, 22, 23, 24, 25, 31, 32, 33, 34, 35, 36, 37]))
Before (3, deque([23, 27, 31, 32, 33, 34, 35, 36]))
Before (2, deque([31, 32, 33, 34, 35, 36, 37]))
Before (1, deque([34, 35, 36, 37, 38, 44, 45, 46]))
Before (3, deque([36, 40, 44, 45, 46, 47, 48]))
Before (2, deque([44, 45, 46, 47, 48, 49, 50]))
Before (1, deque([47, 48, 49, 50, 51]))
Before (3, deque([49, 53, 57]))
Before (2, deque([57, 58, 59, 60, 61]))

After2 (3, deque([1, 5, 6, 7, 8, 9, 10, 14, 18, 19, 20]))
After3 (2, deque([5, 6, 7, 8, 9, 10, 11, 18, 19, 20, 21, 22]))
After3 (1, deque([8, 9, 10, 11, 12, 18, 19, 20, 21, 22, 23, 24, 25]))
After3 (3, deque([14, 18, 19, 20, 21, 22, 23, 27]))
After3 (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34]))
After3 (1, deque([21, 22, 23, 24, 25, 31, 32, 33, 34]))
After3 (3, deque([27, 31, 32, 33]))
After3 (2, deque([31, 32, 33, 34, 35, 36, 37]))
After3 (1, deque([34, 35, 36, 37, 38]))
After3 (3, deque([40]))
After3 (2, deque([44, 45, 46, 47, 48]))
After3 (1, deque([47, 48, 49, 50, 51]))
After3 (3, deque([53]))
After3 (2, deque([57, 58, 59, 60]))

After3 (1, deque([60, 61, 62, 63]))

18313994344847

Before (3, deque([1]))
Before (2, deque([5, 6, 7, 8, 9, 10, 11, 18, 19, 20, 21, 22, 23, 24]))
Before (1, deque([8, 9, 10, 11, 12, 18, 19, 20, 21, 22, 23, 24, 25]))
Before (3, deque([10, 14]))
Before (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34]))

After1 (3, deque([1]))
After2 (2, deque([5, 6, 7, 8, 9, 10, 11]))
After2 (1, deque([8, 9, 10, 11, 12]))
After2 (3, deque([10, 14]))
After2 (2, deque([18, 19, 20, 21, 22, 23, 24]))

Before (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34])) (1, deque([21, 22, 23, 24, 25, 31, 32, 33, 34, 35, 36, 37]))
After1 (2, deque([18, 19, 20, 21, 22, 23, 24, 31, 32, 33, 34])) (1, deque([21, 22, 23, 24, 25, 31, 32, 33, 34, 35, 36, 37]))
After2 (2, deque([18, 19, 20, 21, 22, 23, 24])) (3, deque([10, 14]))

Before (1, deque([21, 22, 23, 24, 25, 31, 32, 33, 34, 35, 36, 37])) (3, deque([23, 27]))
After1 (1, deque([21, 22, 23, 24, 25])) (3, deque([23, 27]))
After2 (1, deque([21, 22, 23, 24, 25])) (2, deque([18, 19, 20, 21, 22, 23, 24]))

Before (3, deque([23, 27])) (2, deque([31, 32, 33, 34, 35, 36, 37]))
After1 (3, deque([23, 27])) (2, deque([31, 32, 33, 34, 35, 36, 37]))
After2 (3, deque([23, 27])) (1, deque([21, 22, 23, 24, 25]))

Before (2, deque([31, 32, 33, 34, 35, 36, 37])) (1, deque([34, 35, 36, 37, 38, 44, 45, 46]))
After1 (2, deque([31, 32, 33, 34, 35, 36, 37])) (1, deque([34, 35, 36, 37, 38, 44, 45, 46]))
After2 (2, deque([31, 32, 33, 34, 35, 36, 37])) (3, deque([23, 27]))

Before (1, deque([34, 35, 36, 37, 38, 44, 45, 46])) (3, deque([36, 40]))
After1 (1, deque([34, 35, 36, 37, 38])) (3, deque([36, 40]))
After2 (1, deque([34, 35, 36, 37, 38])) (2, deque([31, 32, 33, 34, 35, 36, 37]))

Before (3, deque([36, 40])) (2, deque([44, 45, 46, 47, 48, 49, 50]))
After1 (3, deque([36, 40])) (2, deque([44, 45, 46, 47, 48, 49, 50]))
After2 (3, deque([36, 40])) (1, deque([34, 35, 36, 37, 38]))

Before (2, deque([44, 45, 46, 47, 48, 49, 50])) (1, deque([47, 48, 49, 50, 51]))
After1 (2, deque([44, 45, 46, 47, 48])) (1, deque([47, 48, 49, 50, 51]))
After2 (2, deque([44, 45, 46, 47, 48])) (3, deque([36, 40]))

Before (1, deque([47, 48, 49, 50, 51])) (3, deque([49, 53]))
After1 (1, deque([47, 48, 49, 50, 51])) (3, deque([49, 53]))
After2 (1, deque([47, 48, 49, 50, 51])) (2, deque([44, 45, 46, 47, 48]))

Before (3, deque([49, 53])) (2, deque([57, 58, 59, 60, 61]))
After1 (3, deque([49, 53])) (2, deque([57, 58, 59, 60, 61]))
After2 (3, deque([49, 53])) (1, deque([47, 48, 49, 50, 51]))

Before (2, deque([57, 58, 59, 60, 61])) (1, deque([60, 61, 62, 63]))
After1 (2, deque([57, 58, 59, 60])) (1, deque([60, 61, 62, 63]))
After2 (2, deque([57, 58, 59, 60])) (3, deque([49, 53]))

After2 (1, deque([60, 61, 62, 63])) (2, deque([57, 58, 59, 60]))

.??#.?????..????#?.?.??#.?????..????#?.?.??#.?????..????#?.?.??#.?????..????#?.?.??#.?????..????#?. [1, 1, 5, 1, 1, 5, 1, 1, 5, 1, 1, 5, 1, 1, 5]

0
0 .? #. False
1 ?? #. True
2 ?# #. False
3 #. #. True
3
3 #. #. True
5
5 ?????. #####. True
6 ????.. #####. False
7 ???..? #####. False
8 ??..?? #####. False
9 ?..??? #####. False
10 ..???? #####. False
11 .????# #####. False
12 ????#? #####. True
13 ???#?. #####. True
14 ??#?.? #####. False
15 ?#?.?. #####. False
16 #?.?.? #####. False
18 .?.??# #####. False
19 ?.??#. #####. False
20 .??#.? #####. False
21 ??#.?? #####. False
22 ?#.??? #####. False
23 #.???? #####. False
25 ?????. #####. True
26 ????.. #####. False
27 ???..? #####. False
28 ??..?? #####. False
29 ?..??? #####. False
30 ..???? #####. False
31 .????# #####. False
32 ????#? #####. True
33 ???#?. #####. True
34 ??#?.? #####. False
35 ?#?.?. #####. False
36 #?.?.? #####. False
38 .?.??# #####. False
39 ?.??#. #####. False
40 .??#.? #####. False
41 ??#.?? #####. False
42 ?#.??? #####. False
43 #.???? #####. False
45 ?????. #####. True
46 ????.. #####. False
47 ???..? #####. False
48 ??..?? #####. False
49 ?..??? #####. False
50 ..???? #####. False
51 .????# #####. False
52 ????#? #####. True
53 ???#?. #####. True
54 ??#?.? #####. False
55 ?#?.?. #####. False
11
11 .? #. False
12 ?? #. True
13 ?? #. True
14 ?? #. True
15 ?# #. False
16 #? #. True
14
14 ?? #. True
15 ?# #. False
16 #? #. True
16
16 #?.?.? #####. False
18 .?.??# #####. False
19 ?.??#. #####. False
20 .??#.? #####. False
21 ??#.?? #####. False
22 ?#.??? #####. False
23 #.???? #####. False
25 ?????. #####. True
26 ????.. #####. False
27 ???..? #####. False
28 ??..?? #####. False
29 ?..??? #####. False
30 ..???? #####. False
31 .????# #####. False
32 ????#? #####. True
33 ???#?. #####. True
34 ??#?.? #####. False
35 ?#?.?. #####. False
36 #?.?.? #####. False
38 .?.??# #####. False
39 ?.??#. #####. False
40 .??#.? #####. False
41 ??#.?? #####. False
42 ?#.??? #####. False
43 #.???? #####. False
45 ?????. #####. True
46 ????.. #####. False
47 ???..? #####. False
48 ??..?? #####. False
49 ?..??? #####. False
50 ..???? #####. False
51 .????# #####. False
52 ????#? #####. True
53 ???#?. #####. True
54 ??#?.? #####. False
55 ?#?.?. #####. False
56 #?.?.? #####. False
58 .?.??# #####. False
59 ?.??#. #####. False
60 .??#.? #####. False
61 ??#.?? #####. False
62 ?#.??? #####. False
63 #.???? #####. False
65 ?????. #####. True
31
31 .? #. False
32 ?? #. True
33 ?? #. True
34 ?? #. True
35 ?# #. False
36 #? #. True
34
34 ?? #. True
35 ?# #. False
36 #? #. True
36
36 #?.?.? #####. False
38 .?.??# #####. False
39 ?.??#. #####. False
40 .??#.? #####. False
41 ??#.?? #####. False
42 ?#.??? #####. False
43 #.???? #####. False
45 ?????. #####. True
46 ????.. #####. False
47 ???..? #####. False
48 ??..?? #####. False
49 ?..??? #####. False
50 ..???? #####. False
51 .????# #####. False
52 ????#? #####. True
53 ???#?. #####. True
54 ??#?.? #####. False
55 ?#?.?. #####. False
56 #?.?.? #####. False
58 .?.??# #####. False
59 ?.??#. #####. False
60 .??#.? #####. False
61 ??#.?? #####. False
62 ?#.??? #####. False
63 #.???? #####. False
65 ?????. #####. True
66 ????.. #####. False
67 ???..? #####. False
68 ??..?? #####. False
69 ?..??? #####. False
70 ..???? #####. False
71 .????# #####. False
72 ????#? #####. True
73 ???#?. #####. True
74 ??#?.? #####. False
75 ?#?.?. #####. False
51
51 .? #. False
52 ?? #. True
53 ?? #. True
54 ?? #. True
55 ?# #. False
56 #? #. True
54
54 ?? #. True
55 ?# #. False
56 #? #. True
56
56 #?.?.? #####. False
58 .?.??# #####. False
59 ?.??#. #####. False
60 .??#.? #####. False
61 ??#.?? #####. False
62 ?#.??? #####. False
63 #.???? #####. False
65 ?????. #####. True
66 ????.. #####. False
67 ???..? #####. False
68 ??..?? #####. False
69 ?..??? #####. False
70 ..???? #####. False
71 .????# #####. False
72 ????#? #####. True
73 ???#?. #####. True
74 ??#?.? #####. False
75 ?#?.?. #####. False
76 #?.?.? #####. False
78 .?.??# #####. False
79 ?.??#. #####. False
80 .??#.? #####. False
81 ??#.?? #####. False
82 ?#.??? #####. False
83 #.???? #####. False
85 ?????. #####. True
71
71 .? #. False
72 ?? #. True
73 ?? #. True
74 ?? #. True
75 ?# #. False
76 #? #. True
74
74 ?? #. True
75 ?# #. False
76 #? #. True
76
76 #?.?.? #####. False
78 .?.??# #####. False
79 ?.??#. #####. False
80 .??#.? #####. False
81 ??#.?? #####. False
82 ?#.??? #####. False
83 #.???? #####. False
85 ?????. #####. True
86 ????.. #####. False
87 ???..? #####. False
88 ??..?? #####. False
89 ?..??? #####. False
90 ..???? #####. False
91 .????# #####. False
92 ????#? #####. True
93 ???#?. #####. True
94 ??#?.. #####. False
95 ?#?.. #####. False

.??#.?????..????#?.?.??#.?????..????#?.?.??#.?????..????#?.?.??#.?????..????#?.?.??#.?????..????#?. [1, 1, 5, 1, 1, 5, 1, 1, 5, 1, 1, 5, 1, 1, 5]
                                                  #.#.#####.#.#.#####.#.#.#####.#.#.#####.#.#.#####
 1

Before (1, deque([1, 3])) (1, deque([3]))
After1 (1, deque([1])) (1, deque([3]))
After2 (1, deque([1])) (1, deque([3]))

Before (1, deque([3])) (5, deque([5, 12, 13, 25, 32, 33, 45, 52, 53]))
After1 (1, deque([3])) (5, deque([5, 12, 13, 25, 32, 33, 45, 52, 53]))
After2 (1, deque([3])) (5, deque([5, 12, 13, 25, 32, 33, 45, 52, 53]))
After3 (1, deque([3])) (1, deque([1]))

Before (5, deque([5, 12, 13, 25, 32, 33, 45, 52, 53])) (1, deque([12, 13, 14, 16]))
After1 (5, deque([5])) (1, deque([12, 13, 14, 16]))
After2 (5, deque([5])) (1, deque([12, 13, 14, 16]))
After3 (5, deque([5])) (1, deque([3]))

Before (1, deque([12, 13, 14, 16])) (1, deque([14, 16]))
After1 (1, deque([12, 13, 14])) (1, deque([14, 16]))
After2 (1, deque([12, 13, 14])) (1, deque([14, 16]))
After3 (1, deque([12, 13, 14])) (5, deque([5]))

Before (1, deque([14, 16])) (5, deque([25, 32, 33, 45, 52, 53, 65]))
After1 (1, deque([14, 16])) (5, deque([25, 32, 33, 45, 52, 53, 65]))

## First and second try using (slow) search method

```python
def goodsofar(s:str, l:list) -> bool: # check if s matches l until first '?'
    sofar0 = list(map(len, s.split('?', maxsplit=1)[0].replace('.',' ').split())) # list of hash group lengths up until first '?'
    # print ('mapped:', sofar, s.split('?', maxsplit=1)[0], s ) 
  
    # alternative sofar impl without split and replace - still slow
    sofar = []
    end = s.find('?')
    if end<0: end=len(s)
    hs = s.find('#')
    he = 0
    while hs >= 0 and hs < end:
        he = s.find('.',hs)
        if he < 0: he = end
        sofar.append(min(he,end)-hs)
        hs = s.find('#',he+1)
  
    if sofar != sofar0:
        print('Error!',sofar0, sofar)
    # end alternative sofar

    lt = False
    for z in itertools.zip_longest(sofar,l, fillvalue=0):
        if z[0] > z[1]: return False # hash group is bigger than list
        if z[0] > 0 and z[0] < z[1]:
            if lt: return False # hash group is smaller and is not last in list
            else: 
                lt = True # We are at the first less than
                continue

    return True

def good(s:str, l:list) -> bool:
    return l == list(map(len, s.replace('.',' ').split())) # list of hash group lengths up until first '?'

def search(s:str, l:list):
    if s.find('?') < 0: return good(s,l)
    #  print('validcomb:',s,l)
    #  return 1 # no more ? means this combination works

    dot = s.replace('?','.',1)
    hsh = s.replace('?','#',1)
    tot = 0

    dotok = goodsofar(dot, l)
    # print(dot, l, dotok)
    if dotok:
        tot += search(dot, l)
    hashok = goodsofar(hsh, l)
    # print(hsh, l, hashok)
    if hashok:
        tot+= search(hsh, l)

    return tot
```
