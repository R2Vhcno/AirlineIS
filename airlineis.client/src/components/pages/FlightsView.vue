<script setup>
    import { ref, watch } from 'vue';
    import { useRouter } from 'vue-router';
    import { fetchDelete, fetchGet, fetchGetId, fetchGetIdBool, fetchPost, fetchPut } from '@/fetchutils.js'

    let router = useRouter();

    let flightsList = ref(null);
    let airportsList = ref(null);

    let selectedOrigin = ref(0);
    let selectedDestination = ref(0);

    let selectedFlight = ref(0);
    let selectedFlightDeletable = ref(false);

    let state = ref("loading");

    watch(() => selectedFlight.value, async (newId, oldId) => {
        selectedFlightDeletable.value = true;

        await fetchGetIdBool('/api/flights/candelete', newId, {
            success: (response, body) => {
                selectedFlightDeletable.value = body;
            }
        });
    });

    function isLoading() {
        return state.value == 'loading';
    }

    function isIdle() {
        return state.value == 'idle';
    }

    function isError() {
        return state.value == 'error';
    }

    async function getAllFlights() {
        flightsList.value = null;
        airportsList.value = null;
        state.value = "loading";
        selectedFlight.value = 0;

        await fetchGet('api/flights', {
            success: (response, body) => {
                flightsList.value = body;
            },
            error: (response, body) => {
                state.value = "error";
            },
        });

        if (isError()) return;

        await fetchGet('api/airports', {
            success: (response, body) => {
                airportsList.value = body;

                state.value = "idle";
            },
            error: (response, body) => {
                state.value = "error";
            },
        });

    }

    async function filterFlights(originId, destinationId) {
        flightsList.value = null;
        selectedFlight.value = 0;
        state.value = "loading";        

        let request = '/api/flights';

        if (originId != 0 || destinationId != 0) {
            request += '?';
        }

        if (originId != 0) {
            request += `originId=${originId}`;
        }

        if (originId != 0 || destinationId != 0) {
            request += '&';
        }

        if (destinationId != 0) {
            request += `destinationId=${destinationId}`;
        }

        await fetchGet(request, {
            success: (response, body) => {
                flightsList.value = body;
            },
            error: (response, body) => {
                state.value = "error";
            },
        });

        state.value = "idle";
    }

    function bookFlight() {
        let id = selectedFlight.value;

        router.push({ path: `/book/${id}` });
    }

    function createFlight() {
        router.push({ path: `/editflight/0` });
    }

    function editFlight() {
        let id = selectedFlight.value;

        router.push({ path: `/editflight/${id}` });
    }

    async function deleteFlight() {
        let id = selectedFlight.value;

        if (!selectedFlightDeletable.value) return;

        await fetchDelete('/api/flights', id, {
            success: (r, b) => {
                getAllFlights();
            }
        });
    }

    getAllFlights();
</script>

<template>
    <h2>Рейсы</h2>

    <div class="form-line">
        <button @click="getAllFlights()" :disabled="isLoading()">Обновить данные</button>
        <button v-if="selectedFlight!=0" @click="bookFlight()">Оформить билет</button>
        <button @click="createFlight()" :disabled="!isIdle()">Создать рейс</button>
        <button v-if="selectedFlight!=0" @click="editFlight()">Изменить данные рейса</button>
        <button v-if="selectedFlight!=0" @click="deleteFlight()" :disabled="!selectedFlightDeletable">Удалить рейс</button>
    </div>

    <div class="v-spacer"></div>

    <div class="form form-group">
        <h3>Фильтрация</h3>

        <label for="origin-select">Аэропорт отправления:</label>
        <select id="origin-select" v-model="selectedOrigin">
            <option value="0">Все аэропорты</option>
            <option v-for="airport in airportsList" :value="airport.id">
                {{ airport.name }} ({{ airport.code }})
            </option>
        </select>


        <label for="destination-select">Аэропорт назначения:</label>
        <select id="destination-select" v-model="selectedDestination">
            <option value="0">Все аэропорты</option>
            <option v-for="airport in airportsList" :value="airport.id">
                {{ airport.name }} ({{ airport.code }})
            </option>
        </select>

        <div class="v-spacer"></div>

        <button @click="filterFlights(selectedOrigin, selectedDestination)" :disabled="!isIdle()">Поиск</button>
    </div>

    <div class="v-spacer"></div>

    <div v-if="selectedFlight!=0 && !selectedFlightDeletable">
        <div class="alert">
            <div class="badge">инфо</div>
            Данный рейс нельзя удалить, так как на него есть оформленные билеты.
        </div>

        <div class="v-spacer"></div>
    </div>

    <table>
        <thead>
            <tr>
                <th></th>
                <th>Аэропорт отправления</th>
                <th>Аэропорт назначения</th>
                <th>Кол-во мест</th>
                <th>Время отправления</th>
                <th>Время прибытия</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="flight in flightsList" :key="flight.id">
                <td>
                    <input type="radio" v-model="selectedFlight" name="flights-table" :value="flight.id" />
                </td>
                <td>
                    <strong>{{ flight.originCode }}, </strong>
                    <i>{{ flight.originName }}</i>
                </td>
                <td>
                    <strong>{{ flight.destinationCode }}, </strong>
                    <i>{{ flight.destinationName }}</i>
                </td>
                <td>{{ flight.availableSeats }}</td>
                <td>{{ new Date(flight.departureTime).toLocaleString('ru-RU') }}</td>
                <td>{{ new Date(flight.arrivalTime).toLocaleString('ru-RU') }}</td>
            </tr>
            <tr v-if="flightsList != null && flightsList.length==0">
                <td></td>
                <td colspan="5">Список пуст</td>
            </tr>
            <tr v-if="!flightsList && isLoading()">
                <td></td>
                <td colspan="5">Получение списка рейсов...</td>
            </tr>
            <tr v-if="isError()">
                <td></td>
                <td colspan="5">Ошибка при получении списка рейсов. Попробуйте обновить данные через некоторое время.</td>
            </tr>
        </tbody>
    </table>

</template>

<style scoped>
</style>