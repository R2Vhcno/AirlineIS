<script setup>
    import { ref, watch } from 'vue';
    import { fetchDelete, fetchGet, fetchGetId, fetchPost, fetchPut } from '@/fetchutils';

    let classes = ref(null);
    let selectedClass = ref(0);

    let className = ref("");

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

    watch(() => selectedClass.value, async (newId, oldId) => {
        await fetchGetId('/api/classes', newId, {
            success: (response, body) => {
                className.value = body.className;
            }
        });
    });

    async function getClasses() {
        classes.value = null;
        selectedClass.value = 0;
        state.value = "loading";

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

    async function addClass() {
        let klass = {
            "className": "n/a",
        };

        await fetchPost('/api/classes', klass, {
            success: (r, b) => {
                selectedClass.value = b.id;

                getClasses();
            }
        })
    }

    async function updateClass() {
        let id = selectedClass.value;

        let klass = {
            id: id,
            className: className.value,
        }

        await fetchPut('/api/classes', id, klass, {
            success: (response, body) => {
                getClasses();
            }
        });
    }

    async function deleteClass() {
        let id = selectedClass.value;

        await fetchDelete('/api/classes', id, {
            success: (response, body) => {
                getClasses();
            }
        });
    }

    getClasses();
</script>

<template>
    <h2>Классы</h2>

    <button @click="getClasses()" :disabled="isLoading()">Обновить данные</button>
    <button @click="addClass()" :disabled="!isIdle()">Добавить</button>

    <div class="v-spacer"></div>

    <div v-if="isError()">
        <div class="alert">
            <div class="badge">ошибка</div>
            Не удалось получить список аэропортов. Попробуйте обновить данные через некоторое время.
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
            <tr v-for="entry in classes" :key="entry.id">
                <td>
                    <input v-model="selectedClass" type="radio" name="classes-table" :value="entry.id" />
                </td>
                <td>{{ entry.className }}</td>
            </tr>
            <tr v-if="isLoading()">
                <td></td>
                <td colspan="2">Получение списка классов...</td>
            </tr>
            <tr v-if="classes != null && isIdle() && classes.length == 0">
                <td></td>
                <td colspan="2">Список пуст</td>
            </tr>
            <tr v-if="isIdle() && !classes">
                <td></td>
                <td colspan="2">Ошибка</td>
            </tr>
        </tbody>
    </table>

    <div class="v-spacer"></div>

    <div class="form form-group" v-if="selectedClass != 0">
        <h3>Редактирование класса</h3>

        <label for="class-name">Название:</label>
        <input id="class-name" type="text" v-model="className" />

        <div class="v-spacer"></div>

        <div>
            <button @click="updateClass()">Изменить</button>
            <button @click="deleteClass()">Удалить</button>
        </div>
    </div>
</template>

<style scoped>
</style>