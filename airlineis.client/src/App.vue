<template>
    <header>
        <h1>АИС Авиалиния</h1>
        <nav>
            <RouterLink to="/">Рейсы</RouterLink>
            <RouterLink to="/tickets">Билеты</RouterLink>
            <RouterLink to="/airports">Аэропорты</RouterLink>
            <RouterLink to="/planes">Самолёты</RouterLink>
            <RouterLink to="/classes">Классы</RouterLink>
        </nav>
    </header>

    <main>
        <RouterView />
    </main>
</template>