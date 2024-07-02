<script setup>
    import { ref, watch } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import { fetchDelete, fetchGet, fetchGetId, fetchPost, fetchPut } from '@/fetchutils.js'

    let route = useRoute();

    let flightInfo = ref(null);
    let flightDepartureTime = ref(new Date());
    let flightDuration = ref("");

    let originInfo = ref(null);
    let destinationInfo = ref(null);

    // passengers
    let passengers = ref(null);
    let selectedPassenger = ref(0);

    let passengerFirstName = ref("");
    let passengerSurName = ref("");
    let passengerLastName = ref("");
    let passengerPhone = ref("");
    let passengerEmail = ref("");
    let passengerBirthdate = ref("");

    // seats
    let seats = ref(null);
    let selectedSeat = ref(0);

    watch(() => route.params.flightid, (newId, oldId) => {
        initForm();
    });

    watch(() => selectedPassenger.value, async (newId, oldId) => {
        await fetchGetId('/api/passengers', newId, {
            success: (response, body) => {
                passengerFirstName.value = body.firstName;
                passengerSurName.value = body.surName;
                passengerLastName.value = body.lastName;
                passengerPhone.value = body.phone;
                passengerEmail.value = body.email;
                passengerBirthdate.value = body.birthDate;
            }
        });
    });

    async function initForm() {
        let id = new Number(route.params.flightid);

        await fetchGetId('/api/flights', id, {
            ok: (response, body) => {
                flightInfo.value = body;
            }
        });

        await fetchGetId('/api/airports', flightInfo.value.originId, {
            ok: (response, body) => {
                originInfo.value = body;
            }
        });

        await fetchGetId('/api/airports', flightInfo.value.destinationId, {
            ok: (response, body) => {
                destinationInfo.value = body;
            }
        });

        await fetchGet('/api/passengers', {
            ok: (response, body) => {
                passengers.value = body;
            }
        });

        await fetchGetId('/api/planeseats/available', id, {
            ok: (response, body) => {
                seats.value = body;
            }
        });

        // Other useful info
        flightDepartureTime.value = new Date(flightInfo.value.departureTime);
        flightDuration.value = flightInfo.value.duration;
    }

    async function updatePassenger() {
        let id = selectedPassenger.value;

        let passenger = {
            id: id,
            surName: passengerSurName.value,
            firstName: passengerFirstName.value,
            lastName: passengerLastName.value,
            phone: passengerPhone.value,
            email: passengerEmail.value,
            birthDate: passengerBirthdate.value,
        }

        await fetchPut("/api/passengers", id, passenger, {
            success: (response, body) => {
                initForm();
            }
        });
    }

    async function deletePassenger() {
        let id = selectedPassenger.value;

        await fetchDelete('/api/passengers', id, {
            success: (response, body) => {
                selectedPassenger.value = 0;

                initForm();
            }
        });
    }

    async function createNewPassenger() {
        let newPassenger = {
            surName: "Фамилия",
            firstName: "Имя",
            lastName: "Отчество",
            phone: "89999999999",
            email: "example@example.com"
        };

        await fetchPost('/api/passengers', newPassenger, {
            success: (response, body) => {
                selectedPassenger.value = body.id;
                passengerFirstName.value = body.firstName;
                passengerSurName.value = body.surName;
                passengerLastName.value = body.lastName;
                passengerPhone.value = body.phone;
                passengerEmail.value = body.email;
                passengerBirthdate.value = body.birthDate;

                initForm();
            }
        });
    }

    async function bookFlight() {
        let router = useRouter();

        let newTicket = {
            flightId: flightInfo.value.id,
            planeSeatId: selectedSeat.value,
            passengerId: selectedPassenger.value
        };

        await fetchPost('/api/tickets', newTicket, {
            success: (response, body) => {
                router.push('/');
            },
            error: (response, body) => {
                alert(response.status);
                alert(response.ok);
            }
        });
    }

    initForm();
</script>

<template>
    <h2>Оформление билета</h2>


    <div class="form">
        <div class="form form-group">
            <h3>Маршрут</h3>

            <div class="form-text">
                Аэропорт отправления:
                &nbsp;
                <strong>
                    {{ originInfo.name }}
                </strong>
                &nbsp;
                Время отправления:
                &nbsp;
                <strong>
                    {{ flightDepartureTime.toLocaleString('ru-RU') }}
                </strong>
            </div>

            <div class="form-text">
                Аэропорт прибытия:
                &nbsp;
                <strong>
                    {{ destinationInfo.name }}
                </strong>
                &nbsp;
                Длительность полёта:
                &nbsp;
                <strong>
                    {{ flightDuration }}
                </strong>
            </div>
        </div>

        <div class="v-spacer"></div>

        <div class="form form-group">
            <h3>Пассажир</h3>

            <div v-if="selectedPassenger == 0" class="alert">
                <div class="badge">информация</div>
                Необходимо выбрать клиента.
            </div>

            <div class="v-spacer"></div>

            <table>
                <thead>
                    <tr>
                        <th></th>
                        <th>Фамилия</th>
                        <th>Имя</th>
                        <th>Отчество</th>
                        <th>Телефон</th>
                        <th>Эл. почта</th>
                        <th>Дата рождения</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="entry in passengers" :key="entry.id">
                        <td>
                            <input v-model="selectedPassenger" type="radio" name="passengers-table" :value="entry.id" />
                        </td>
                        <td>{{ entry.surName }}</td>
                        <td>{{ entry.firstName }}</td>
                        <td>{{ entry.lastName }}</td>
                        <td>{{ entry.phone }}</td>
                        <td>{{ entry.email }}</td>
                        <td>{{ new Date(entry.birthDate).toLocaleDateString('ru-RU') }}</td>
                    </tr>
                    <tr v-if="!passengers">
                        <td></td>
                        <td colspan="5">Получение списка клиентов из базы данных...</td>
                    </tr>
                </tbody>
            </table>

            <div class="v-spacer"></div>

            <button @click="createNewPassenger()">Добавить нового клиента</button>

            <div v-if="selectedPassenger != 0" class="form form-group">
                <h3>Редактирование информации</h3>

                <label for="pass-surname">Фамилия:</label>
                <input id="pass-surname" type="text" v-model="passengerSurName" />

                <label for="pass-firstname">Имя:</label>
                <input id="pass-firstname" type="text" v-model="passengerFirstName" />

                <label for="pass-lastname">Отчество:</label>
                <input id="pass-lastname" type="text" v-model="passengerLastName" />

                <label for="pass-birthdate">E-mail:</label>
                <input id="pass-birthdate" type="date" v-model="passengerBirthdate" />

                <div class="form-separator"></div>

                <label for="pass-phone">Телефон:</label>
                <input id="pass-phone" type="text" v-model="passengerPhone" />

                <label for="pass-email">E-mail:</label>
                <input id="pass-email" type="text" v-model="passengerEmail" />

                <div>
                    <button @click="updatePassenger()">Изменить</button>
                    <button @click="deletePassenger()">Удалить</button>
                </div>
            </div>
        </div>

        <div class="v-spacer"></div>

        <div class="form form-group">
            <h3>Место</h3>

            <div v-if="selectedSeat == 0" class="alert">
                <div class="badge">информация</div>
                Необходимо выбрать место.
            </div>

            <div class="v-spacer"></div>

            <table>
                <thead>
                    <tr>
                        <th></th>
                        <th>Название</th>
                        <th>Класс</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="entry in seats" :key="entry.id">
                        <td>
                            <input v-model="selectedSeat" type="radio" name="seats-table" :value="entry.id" />
                        </td>
                        <td>{{ entry.seatName }}</td>
                        <td>{{ entry.class.className }}</td>
                    </tr>
                    <tr v-if="!seats">
                        <td></td>
                        <td colspan="5">Получение списка пассажирских мест из базы данных...</td>
                    </tr>
                    <tr v-if="!seats && seats.length == 0">
                        <td></td>
                        <td colspan="5">На данный рейс свободных мест нет.</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="v-spacer"></div>

        <div>
            <button @click="$router.push({ path: '/'})">Отмена</button>
            <button v-if="selectedPassenger != 0 && selectedSeat != 0" @click="bookFlight()">Оформить билет</button>
        </div>
    </div>
</template>

<style scoped>
</style>