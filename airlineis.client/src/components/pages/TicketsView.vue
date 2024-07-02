<script setup>
    import { ref } from 'vue';
    import { fetchDelete, fetchGet } from '@/fetchutils';

    let tickets = ref(null);
    let selectedTicket = ref(0);

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

    async function getTicketsList() {
        tickets.value = null;
        state.value = "loading";

        await fetchGet("/api/pnrs", {
            success: (response, body) => {
                tickets.value = body;

                state.value = "idle";
            },
            error: (response, body) => {
                state.value = "error";
            }
        });
    }

    async function deleteTicket() {
        let id = selectedTicket.value;

        await fetchDelete("api/tickets", id, {
            success: (response, body) => {
                getTicketsList();
            }
        });
    }

    getTicketsList();
</script>

<template>
    <h2>Билеты</h2>

    <button @click="getTicketsList()" :disabled="isLoading()">Обновить данные</button>
    <button @click="deleteTicket()" :disabled="!isIdle()">Удалить</button>

    <div class="v-spacer"></div>

    <div v-if="isError()">
        <div class="alert">
            <div class="badge">ошибка</div>
            Не удалось получить список билетов. Попробуйте обновить данные через некоторое время.
        </div>

        <div class="v-spacer"></div>
    </div>

    <table>
        <thead>
            <tr>
                <th></th>
                <th>Пассажир</th>
                <th>Дата рождения</th>
                <th>Контакты</th>
                <th>Место</th>
                <th>Откуда</th>
                <th>Куда</th>
                <th>Дата</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="entry in tickets" :key="entry.id">
                <td>
                    <input v-model="selectedTicket" type="radio" name="tickets-table" :value="entry.id" />
                </td>
                <td>{{ entry.name }}</td>
                <td>{{ entry.birthDate }}</td>
                <td>{{ entry.contactInfo }}</td>
                <td>{{ entry.seatName }}</td>
                <td>
                    <strong>{{ entry.originCode }}</strong>
                    , {{ entry.originName}}
                </td>
                <td>
                    <strong>{{ entry.destinationCode }}</strong>
                    , {{ entry.destinationName}}
                </td>
                <td>{{ new Date(entry.departureTime).toLocaleString('ru-RU') }}</td>
            </tr>
            <tr v-if="isLoading()">
                <td></td>
                <td colspan="2">Получение списка аэропортов...</td>
            </tr>
            <tr v-if="tickets != null && isIdle() && tickets.length == 0">
                <td></td>
                <td colspan="2">Список пуст</td>
            </tr>
            <tr v-if="isIdle() && !tickets">
                <td></td>
                <td colspan="2">Ошибка</td>
            </tr>
        </tbody>
    </table>

</template>

<style scoped>
</style>