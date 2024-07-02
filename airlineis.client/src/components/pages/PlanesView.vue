<script setup>
    import { ref, watch } from 'vue';
    import { fetchDelete, fetchGet, fetchGetId, fetchPost, fetchPut, fetchGetIdBool } from '@/fetchutils';

    let planes = ref(null);
    let selectedPlane = ref(0);
    let selectedPlaneDeletable = ref(true);

    let planeName = ref("");

    let planeSeats = ref(null);
    let classes = ref(null);

    let selectedPlaneSeat = ref(0);
    let planeSeatName = ref("");
    let selectedClass = ref(0);

    let state = ref("loading");

    function isLoading() {
        return state.value == 'loading';
    }

    function isIdle() {
        return state.value == 'idle';
    }

    function isError() {
        return state.value == 'error';
    }

    watch(() => selectedPlane.value, async (newId, oldId) => {
        await fetchGetId('/api/planes', newId, {
            success: (response, body) => {
                planeName.value = body.name;

                // Load plane seats
                planeSeats.value = null;
                selectedPlaneSeat.value = 0;

                fetchGetId('/api/planeseats/plane', newId, {
                    success: (r, b) => {
                        planeSeats.value = b;
                    }
                });
            }
        });

        await fetchGetIdBool('/api/planes/candelete', newId, {
            success: (response, body) => {
                selectedPlaneDeletable.value = body;
            }
        });
    });

    watch(() => selectedPlaneSeat.value, async (newId, oldId) => {
        await fetchGetId('/api/planeseats', newId, {
            success: (response, body) => {
                selectedClass.value = body.classId;
                planeSeatName.value = body.seatName;
            }
        });
    });

    async function getPlanesList() {
        planes.value = null;
        selectedPlane.value = 0;
        state.value = "loading";

        await fetchGet('/api/planes', {
            success: (response, body) => {
                planes.value = body;
            },
            error: (response, body) => {
                state.value = "error";
            },
        });

        if (isError()) return;

        await fetchGet('/api/classes', {
            success: (response, body) => {
                classes.value = body;

                state.value = "idle";
            },
            error: (response, body) => {
                state.value = "error";
            },
        });
    }

    async function addNewPlane() {
        let plane = {
            "name": "Самолёт",
        };

        await fetchPost("/api/planes", plane, {
            success: (r, b) => {
                getPlanesList();

                selectedPlane.value = b.id;
            }
        });
    }

    async function updatePlane() {
        let id = selectedPlane.value;

        let plane = {
            id: id,
            name: planeName.value,
        }

        await fetchPut('/api/planes', id, plane, {
            success: (response, body) => {
                let savedId = selectedPlane.value;

                getPlanesList();

                selectedPlane.value = savedId;
            }
        });
    }

    async function deletePlane() {
        let id = selectedPlane.value;

        await fetchDelete('/api/planes', id, {
            success: (response, body) => {
                getPlanesList();
            }
        });
    }

    async function getPlaneSeats() {
        let id = selectedPlane.value;

        planeSeats.value = null;
        selectedPlaneSeat.value = 0;

        fetchGetId('/api/planeseats/plane', id, {
            success: (r, b) => {
                planeSeats.value = b;
            }
        });
    }

    async function addPlaneSeat() {
        let id = selectedPlane.value;

        let seat = {
            seatName: "n/a",
            planeId: id,
            classId: classes.value[0].id,
        };

        await fetchPost('/api/planeseats', seat, {
            success: (r, b) => {
                getPlaneSeats();

                selectedPlaneSeat.value = b.id;
            }
        });
    }

    async function updatePlaneSeat() {
        let planeId = selectedPlane.value;
        let seatId = selectedPlaneSeat.value;

        let seat = {
            id: seatId,
            planeId: planeId,
            seatName: planeSeatName.value,
            classId: selectedClass.value,
        };

        await fetchPut('/api/planeseats', seatId, seat, {
            success: (r, b) => {
                getPlaneSeats();

                selectedPlaneSeat.value = seatId;
            }
        });
    }

    async function deletePlaneSeat() {
        let id = selectedPlaneSeat.value;

        await fetchDelete('/api/planeseats', id, {
            success: (r, b) => {
                getPlaneSeats();
            }
        });
    }

    getPlanesList();
</script>

<template>
    <h2>Самолёты</h2>

    <button @click="getPlanesList()" :disabled="isLoading()">Обновить данные</button>
    <button @click="addNewPlane()" :disabled="!isIdle()">Добавить</button>

    <div class="v-spacer"></div>

    <div v-if="selectedPlane!=0 && !selectedPlaneDeletable">
        <div class="alert">
            <div class="badge">инфо</div>
            Данный самолёт нельзя удалить, так как существуют рейсы с ним.
        </div>

        <div class="v-spacer"></div>
    </div>

    <div v-if="isError()">
        <div class="alert">
            <div class="badge">ошибка</div>
            Не удалось получить список самолётов. Попробуйте обновить данные через некоторое время.
        </div>

        <div class="v-spacer"></div>
    </div>

    <table>
        <thead>
            <tr>
                <th></th>
                <th>Название</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="entry in planes" :key="entry.id">
                <td>
                    <input v-model="selectedPlane" type="radio" name="planes-table" :value="entry.id" />
                </td>
                <td>{{ entry.name }}</td>
            </tr>
            <tr v-if="isLoading()">
                <td></td>
                <td colspan="2">Получение списка аэропортов...</td>
            </tr>
            <tr v-if="planes != null && isIdle() && planes.length == 0">
                <td></td>
                <td colspan="1">Список пуст</td>
            </tr>
            <tr v-if="isIdle() && !planes">
                <td></td>
                <td colspan="1">Ошибка</td>
            </tr>
        </tbody>
    </table>

    <div class="v-spacer"></div>

    <div class="form form-group" v-if="selectedPlane != 0">
        <h3>Редактирование самолёта</h3>

        <label for="plane-name">Название:</label>
        <input id="plane-name" type="text" v-model="planeName" />

        <div class="v-spacer"></div>

        <div class="form form-group">
            <h3>Пассажирские места</h3>

            <div v-if="selectedPlane!=0 && !selectedPlaneDeletable">
                <div class="v-spacer"></div>

                <div class="alert">
                    <div class="badge">инфо</div>
                    Редактирование мест недоступно, так как есть оформленные билеты на рейс с участием данного самолёта.
                </div>

                <div class="v-spacer"></div>
            </div>

            <table>
                <thead>
                    <tr>
                        <th></th>
                        <th>Название</th>
                        <th>Класс</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="entry in planeSeats" :key="entry.id">
                        <td>
                            <input v-model="selectedPlaneSeat" type="radio" name="planeseats-table" :value="entry.id" />
                        </td>
                        <td>{{ entry.seatName }}</td>
                        <td>{{ entry.class.className }}</td>
                    </tr>
                    <tr v-if="planeSeats != null && planeSeats.length == 0">
                        <td></td>
                        <td colspan="2">Список пуст</td>
                    </tr>
                    <tr v-if="planeSeats == null">
                        <td></td>
                        <td colspan="2">Ошибка</td>
                    </tr>
                </tbody>
            </table>

            <div class="v-spacer"></div>

            <button @click="addPlaneSeat()" :disabled="!selectedPlaneDeletable">Добавить</button>

            <div v-if="selectedPlaneSeat != 0" class="v-spacer"></div>

            <div v-if="selectedPlaneSeat != 0" class="form form-group">
                <h3>Редактирование места</h3>
                <label for="seat-name">Название:</label>
                <input id="seat-name" type="text" v-model="planeSeatName" />


                <label for="class-select">Класс:</label>
                <select id="class-select" v-model="selectedClass">
                    <option v-for="klass in classes" :value="klass.id">
                        {{ klass.className }}
                    </option>
                </select>

                <div class="v-spacer"></div>

                <div>
                    <button @click="updatePlaneSeat()" :disabled="!selectedPlaneDeletable">Изменить</button>
                    <button @click="deletePlaneSeat()" :disabled="!selectedPlaneDeletable">Удалить</button>
                </div>
            </div>
        </div>

        <div class="v-spacer"></div>

        <div>
            <button @click="updatePlane()">Изменить</button>
            <button @click="deletePlane()" :disabled="!selectedPlaneDeletable">Удалить</button>
        </div>
    </div>
</template>

<style scoped>
</style>