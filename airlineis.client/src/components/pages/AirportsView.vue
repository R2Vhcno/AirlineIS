<script setup>
    import { ref, watch } from 'vue';
    import { fetchDelete, fetchGet, fetchGetId, fetchPut, fetchPost, fetchGetIdBool } from '@/fetchutils';

    let airports = ref(null);
    let selectedAirport = ref(0);
    let selectedAirportDeletable = ref(true);

    let airportName = ref("");
    let airportIATACode = ref("");
    let airportLocalCode = ref("");
    let airportCity = ref("");
    let airportCountry = ref("");

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

    watch(() => selectedAirport.value, async (newId, oldId) => {
        await fetchGetId('/api/airports', newId, {
            success: (response, body) => {
                airportName.value = body.name;
                airportIATACode.value = body.iatacode;
                airportLocalCode.value = body.localCode;
                airportCity.value = body.city;
                airportCountry.value = body.country;
            }
        });

        await fetchGetIdBool('/api/airports/candelete', newId, {
            success: (response, body) => {
                selectedAirportDeletable.value = body;
            }
        });
    });

    async function getAirportsList() {
        airports.value = null;
        selectedAirport.value = 0;
        state.value = "loading";

        await fetchGet('/api/airports', {
            success: (response, body) => {
                airports.value = body;

                state.value = "idle";
            },
            error: (response, body) => {
                state.value = "error";
            },
        });
    }

    async function editSpecificAirport() {
        let id = selectedAirport.value;

        alert(id);


    }

    async function addNewAirport() {
        let defaultAirport = {
            "name": "n/a",
            "city": "Город",
            "country": "Страна",
            "iatacode": "N/A"
        };

        await fetchPost('/api/airports', defaultAirport, {
            success: (response, body) => {
                getAirportsList();

                selectedAirport.value = body.id;
            },
            error: (response, body) => {
                alert(response.response);
            },
        });

    }

    async function updateSpecificAirport() {
        let id = selectedAirport.value;

        let airport = {
            id: id,
            name: airportName.value,
            city: airportCity.value,
            iatacode: null,
            localCode: null,
            country: airportCountry.value,
        }

        if (airportIATACode.value.length != 0) {
            airport.iatacode = airportIATACode.value;
        }

        if (airportLocalCode.value.length != 0) {
            airport.localCode = airportLocalCode.value;
        }

        await fetchPut('/api/airports', id, airport, {
            success: (response, body) => {
                getAirportsList();

                selectedAirport.value = id;
            }
        });
    }

    async function deleteSpecificAirport() {
        let id = selectedAirport.value;

        await fetchDelete('/api/airports', id, {
            success: (response, body) => {
                getAirportsList();
            }
        });
    }

    function isSelected(id) {
        return selectedAirport.value != null && selectedAirport.value.id == id;
    }

    getAirportsList();
</script>

<template>
    <h2>Аэропорты</h2>

    <button @click="getAirportsList()" :disabled="isLoading()">Обновить данные</button>
    <button @click="addNewAirport()" :disabled="!isIdle()">Добавить</button>

    <div class="v-spacer"></div>

    <div v-if="isError()">
        <div class="alert">
            <div class="badge">ошибка</div>
            Не удалось получить список аэропортов. Попробуйте обновить данные через некоторое время.
        </div>

        <div class="v-spacer"></div>
    </div>

    <div v-if="selectedAirport!=0 && !selectedAirportDeletable">
        <div class="alert">
            <div class="badge">инфо</div>
            Данный аэропорт нельзя удалить, так как существуют связанные рейсы.
        </div>

        <div class="v-spacer"></div>
    </div>

    <table>
        <thead>
            <tr>
                <th></th>
                <th>Код</th>
                <th>Название</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="entry in airports" :key="entry.id">
                <td>
                    <input v-model="selectedAirport" type="radio" name="airports-table" :value="entry.id" />
                </td>
                <td>{{ entry.code }}</td>
                <td>{{ entry.name }}</td>
            </tr>
            <tr v-if="isLoading()">
                <td></td>
                <td colspan="2">Получение списка аэропортов...</td>
            </tr>
            <tr v-if="airports != null && isIdle() && airports.length == 0">
                <td></td>
                <td colspan="2">Список пуст</td>
            </tr>
            <tr v-if="isIdle() && !airports">
                <td></td>
                <td colspan="2">Ошибка</td>
            </tr>
        </tbody>
    </table>

    <div class="v-spacer"></div>

    <div class="form form-group" v-if="selectedAirport != 0">
        <h3>Редактирование аэропорта</h3>

        <label for="airport-name">Название:</label>
        <input id="airport-name" type="text" v-model="airportName" />

        <div class="v-spacer"></div>

        <div class=" form form-group">
            <h3>Код</h3>

            <label for="airport-iata">Код IATA:</label>
            <input id="airport-iata" type="text" v-model="airportIATACode" />

            <label for="airport-local">Код (местный):</label>
            <input id="airport-local" type="text" v-model="airportLocalCode" />
        </div>

        <div class="v-spacer"></div>

        <div class="form form-group">
            <h3>Адрес</h3>
            <label for="airport-city">Город:</label>
            <input id="airport-city" type="text" v-model="airportCity" />

            <label for="airport-country">Страна:</label>
            <input id="airport-country" type="text" v-model="airportCountry" />
        </div>

        <div class="v-spacer"></div>

        <div>
            <button @click="updateSpecificAirport()">Изменить</button>
            <button @click="deleteSpecificAirport()" :disabled="!selectedAirportDeletable">Удалить</button>
        </div>
    </div>
</template>