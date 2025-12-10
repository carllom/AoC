import numpy as np
from scipy.optimize import milp, LinearConstraint, Bounds  # use MILP solver, linprog did not work

data = [[line[0][1:-1],  # lights [..#]
         [list(map(int, b[1:-1].split(','))) for b in line[1:-1]],  # buttons (1,2,3)
         list(map(int, line[-1][1:-1].split(',')))]  # target levels {1,2,3}
        for line in [x.split() for x in open('day10-1.data', encoding='utf-8')]]


def press_btn(state: list[bool], button: list[int]) -> list[bool]:
    new_state = list(state)
    for light in button:
        new_state[light] = not new_state[light]
    return new_state


def min_presses(state, buttons):
    num_press = float('inf')
    for pattern in range(1 << len(buttons)):  # all combinations with 0..1 press per button. More presses are redundant
        new_state = list(state)  # copy initial state
        for btn_idx in range(len(buttons)):
            if (pattern >> btn_idx) & 1:
                new_state = press_btn(new_state, buttons[btn_idx])
        if all(not light for light in new_state):
            num_press = min(num_press, pattern.bit_count())
    return num_press if num_press != float('inf') else -1


print('Day10-1:', sum([min_presses([c == '#' for c in light], buttons) for light, buttons, _ in data]))


def solve_lineq(buttons, target):
    eqsys = np.zeros((len(target), len(buttons)))  # Equation system: target equations x button unknowns
    for button_idx, counter_indices in enumerate(buttons):
        for counter_idx in counter_indices:
            eqsys[counter_idx][button_idx] = 1  # Button advances this counter

    c = np.ones(len(buttons))  # Objective: minimize sum of button presses (all coefficients 1 => equivalent weights => minimizes sum)
    constraints = LinearConstraint(eqsys, lb=target, ub=target)  # Must satisfy: A @ x == b (land exactly on target)
    bounds = Bounds(lb=0, ub=np.inf)  # no negative solutions, only x >= 0
    integrality = np.ones(len(buttons))  # 1 = variable must be integer

    result = milp(c=c, constraints=constraints, bounds=bounds, integrality=integrality)  # solve equation system
    return int(np.sum(np.round(result.x).astype(int)))  # Round properly - milp returns float


print('Day10-2:', sum([solve_lineq(buttons, target) for _, buttons, target in data]))
